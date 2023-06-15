using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Commons
{
    /// <summary>
    /// Xử lý mã hóa, giải mã
    /// </summary>
    public interface IEncodeService
    {
        /// <summary>
        /// Method mã hóa chuỗi sử dụng mã hóa AES
        /// </summary>
        /// <param name="plainText">Chuỗi cần mã hóa</param>
        /// <param name="password">ClientSecret của ứng dụng</param>
        /// <param name="salt">ClientID của ứng dụng</param>
        string EncryptAES(string plainText, string password, string salt);

        /// <summary>
        /// Method giải mã chuỗi sử dụng mã hóa AES
        /// </summary>
        /// <param name="encryptedText">Chuỗi cần giải mã</param>
        /// <param name="password">ClientSecret của ứng dụng</param>
        /// <param name="salt">ClientID của ứng dụng</param>
        string DecryptAES(string encryptedText, string password, string salt);

        /// <summary>
        /// Mã hóa base64
        /// </summary>
        /// <param name="plainText">chuỗi đầu vào</param>
        string EncodeBase64(string plainText);

        /// <summary>
        /// Giải mã base64
        /// </summary>
        /// <param name="base64String">chuỗi mã hóa</param>
        string DecodeBase64(string base64String);
    }
}
