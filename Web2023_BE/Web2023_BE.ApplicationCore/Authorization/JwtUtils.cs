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
        public string GenerateJwtToken(string role, string username);
        public string ValidateJwtToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly AuthConfig _authConfig;

        public JwtUtils(AuthConfig authConfig)
        {
            _authConfig = authConfig;
        }

        public string GenerateJwtToken(string role, string username)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfig.JwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Role", role), new Claim("UserName", username) }),
                Expires = DateTime.UtcNow.AddDays(7),
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
            var key = Encoding.ASCII.GetBytes(_authConfig.JwtSettings.Key);
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
                var role = jwtToken.Claims.First(x => x.Type == "Role").Value;

                // return user id from JWT token if validation successful
                return role;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
