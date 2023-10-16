using System.IO;
using PdfSharp.Pdf.Security;
using PdfSharp.Pdf.IO;

namespace ABC.POS.Website.Models
{
	public class PDFHelper
    {
		public static byte[] ProtectPdf(byte[] pdfBytes, string password)
		{
			var document = PdfReader.Open(new MemoryStream(pdfBytes), PdfDocumentOpenMode.Modify);

			// Set a password to protect the PDF
			PdfSecuritySettings securitySettings = document.SecuritySettings;
			securitySettings.UserPassword = password;

			using (var memoryStream = new MemoryStream())
			{
				document.Save(memoryStream);
				return memoryStream.ToArray();
			}
		}
	}
}
