using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABC.POS.Website.Service
{
    public interface ISmsService
    {
        Task<List<string>> SendSmsAsync(List<string> toPhoneNumbers, string messageBody);
    }
}