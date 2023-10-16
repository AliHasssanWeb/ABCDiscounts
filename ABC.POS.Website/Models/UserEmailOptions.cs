﻿using System.Collections.Generic;

namespace ABC.POS.Website.Models
{
    public class UserEmailOptions 
    {
        public List<string> ToEmails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<KeyValuePair<string, string>> PlaceHolders { get; set; }
    }


    public class UserEmailPDFOptions
    {
        public List<string> ToEmails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<EmailAttachment> Attachments { get; set; }
    }

    public class EmailAttachment
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
