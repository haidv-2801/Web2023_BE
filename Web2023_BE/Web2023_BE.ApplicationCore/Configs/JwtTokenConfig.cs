using System;
using System.Collections.Generic;
using System.Text;
using Web2023_BE.ApplicationCore.Entities;

namespace MISA.Legder.Domain.Configs
{

    public class AuthConfig
    {
        /// <summary>
        /// Dùng cho mode debug
        /// case dev phát triển do chạy auth api và client UI khác host -> phải đẩy về client để ghi cookie ở đó
        /// </summary>
        public bool ReturnClientSessionId { get; set; }

        /// <summary>
        /// Tùy chỉnh cookie authen
        /// </summary>
        public AuthCookieOption CookieOption { get; set; }

        /// <summary>
        /// Cấu hình cho jwt
        /// </summary>
        public JwtTokenConfig JwtToken { get; set; }


        public JwtSettings JwtSettings { get; set; }
    }
    public class JwtTokenConfig
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
    }

    public class AuthCookieOption
    {
        public string? Domain { get; set; }
        public string? Path { get; set; }
        //public DateTimeOffset? Expires { get; set; }
        public bool? Secure { get; set; }
        public int? SameSite { get; set; }
        public bool? HttpOnly { get; set; }
        //public TimeSpan? MaxAge { get; set; }
        public bool? IsEssential { get; set; }
    }
}
