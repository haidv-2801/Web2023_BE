
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.Domain.Shared.Commons;

namespace Web2023_BE.Cache
{
    internal class EncodeService : IEncodeService
    {
        /// <summary>
        /// Method mã hóa chuỗi sử dụng mã hóa AES
        /// </summary>
        /// <param name="plainText">Chuỗi cần mã hóa</param>
        /// <param name="password">ClientSecret của ứng dụng</param>
        /// <param name="salt">ClientID của ứng dụng</param>
        public string EncryptAES(string plainText, string password, string salt)
        {
            using (Aes aes = new AesManaged())
            {
                var deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt));
                aes.Key = deriveBytes.GetBytes(128 / 8);
                aes.IV = aes.Key;
                using (MemoryStream encryptionStream = new MemoryStream())
                {
                    using (CryptoStream encrypt = new CryptoStream(encryptionStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] utfD1 = UTF8Encoding.UTF8.GetBytes(plainText);
                        encrypt.Write(utfD1, 0, utfD1.Length);
                        encrypt.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(encryptionStream.ToArray());
                }
            }
        }

        /// <summary>
        /// Method giải mã chuỗi sử dụng mã hóa AES
        /// </summary>
        /// <param name="encryptedText">Chuỗi cần giải mã</param>
        /// <param name="password">ClientSecret của ứng dụng</param>
        /// <param name="salt">ClientID của ứng dụng</param>
        public string DecryptAES(string encryptedText, string password, string salt)
        {
            using (Aes aes = new AesManaged())
            {
                Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt));
                aes.Key = deriveBytes.GetBytes(128 / 8);
                aes.IV = aes.Key;
                using (MemoryStream decryptionStream = new MemoryStream())
                {
                    using (CryptoStream decrypt = new CryptoStream(decryptionStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        byte[] encryptedData = Convert.FromBase64String(encryptedText);
                        decrypt.Write(encryptedData, 0, encryptedData.Length);
                        decrypt.Flush();
                    }
                    byte[] decryptedData = decryptionStream.ToArray();
                    return UTF8Encoding.UTF8.GetString(decryptedData, 0, decryptedData.Length);
                }
            }
        }

        public string EncodeBase64(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string DecodeBase64(string base64String)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64String);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
