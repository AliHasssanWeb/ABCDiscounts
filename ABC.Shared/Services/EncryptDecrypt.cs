using ABC.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Shared.Services
{
    public class EncryptDecrypt : IEncryptDecrypt
    {
        public string Encrypt(string toEncrypt, bool useHashing = true) // To Encrypt
        {
            byte[] KeyValue;
            byte[] EncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            AppSettingsReader settingreader = new AppSettingsReader();
            string Key = "InovediaPakistan";//(string)settingreader.GetValue("Inovedia311", typeof(string));

            if (useHashing)
            {
                MD5CryptoServiceProvider hashing = new MD5CryptoServiceProvider();
                KeyValue = hashing.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key));
                hashing.Clear();
            }
            else
            {
                KeyValue = UTF8Encoding.UTF8.GetBytes(Key);
            }

            TripleDESCryptoServiceProvider triple = new TripleDESCryptoServiceProvider();
            triple.Key = KeyValue;
            triple.Mode = CipherMode.ECB;
            triple.Padding = PaddingMode.PKCS7;

            ICryptoTransform transform = triple.CreateEncryptor();

            byte[] ResultArray = transform.TransformFinalBlock(EncryptArray, 0, EncryptArray.Length);
            triple.Clear();

            return Convert.ToBase64String(ResultArray, 0, ResultArray.Length);
        }

        public string Decrypt(string toDecrypt, bool useHashing = true) // To Decrypt
        {
            if (toDecrypt == "") return "";
            byte[] KeyValue;
            byte[] EncryptArray = Convert.FromBase64String(toDecrypt);

            AppSettingsReader settingreader = new AppSettingsReader();
            string Key = "InovediaPakistan";//(string)settingreader.GetValue("Inovedia311", typeof(string));

            if (useHashing)
            {
                MD5CryptoServiceProvider hashing = new MD5CryptoServiceProvider();
                KeyValue = hashing.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key));
                hashing.Clear();
            }
            else
            {
                KeyValue = UTF8Encoding.UTF8.GetBytes(Key);
            }

            TripleDESCryptoServiceProvider triple = new TripleDESCryptoServiceProvider();
            triple.Key = KeyValue;
            triple.Mode = CipherMode.ECB;
            triple.Padding = PaddingMode.PKCS7;

            ICryptoTransform transform = triple.CreateDecryptor();
            byte[] ResultArray = transform.TransformFinalBlock(EncryptArray, 0, EncryptArray.Length);
            triple.Clear();

            return UTF8Encoding.UTF8.GetString(ResultArray);
        }
    }
}
