using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MISA.Legder.Domain.Configs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Web2023_BE.ApplicationCore.Entities;

namespace Web2023_BE.ApplicationCore.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(string role, string accountID, string username);
        public string ValidateJwtToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly AuthConfig _authConfig;
        private readonly IConfiguration _config;


        public JwtUtils(AuthConfig authConfig, IConfiguration config)
        {
            _authConfig = authConfig;
            _config = config;
        }

        public string GenerateJwtToken(string role, string accountID, string username)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfig.JwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Role", role), new Claim("AccountID", accountID), new Claim("UserName", username) }),
                Expires = DateTime.UtcNow.AddHours(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            //var key = Encoding.ASCII.GetBytes(_authConfig.JwtSettings.Key);
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountID = jwtToken.Claims.First(x => x.Type == "AccountID").Value;

                return accountID;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
