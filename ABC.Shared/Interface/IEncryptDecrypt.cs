using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Shared.Interface
{
   public interface IEncryptDecrypt
    {
        string Encrypt(string toEncrypt, bool useHashing = true);
        string Decrypt(string toDecrypt, bool useHashing = true);


    }
}
