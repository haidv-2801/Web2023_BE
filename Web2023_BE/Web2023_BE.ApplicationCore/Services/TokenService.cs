using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using MISA.Legder.Domain.Configs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web2023_BE.ApplicationCore.Interfaces.IServices;

namespace Web2023_BE.ApplicationCore.Services
{
    public class TokenService : ITokenService
    {
        private string _role = "";
        private readonly AuthConfig _authConfig;

        public TokenService(AuthConfig authConfig)
        {
            _authConfig = authConfig;
        }

       

        private string GenerateJwt() => GenerateEncryptedToken(GetSigningCredential());


        private string GenerateEncryptedToken(SigningCredentials signingCredentials)
        {
            var claims = new[]
            {
            new Claim("Role", _role)
        };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private SigningCredentials GetSigningCredential()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_authConfig.JwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256);
        }

        public TokenResponse GetToken(TokenRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
