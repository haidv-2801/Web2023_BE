using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using MISA.Legder.Domain.Configs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Interfaces.IServices;


namespace Web2023_BE.ApplicationCore.Services
{
    public class TokenService : ITokenService
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthConfig _authConfig;

        public TokenService(IDistributedCache cache, IHttpContextAccessor httpContextAccessor, AuthConfig authConfig)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _authConfig = authConfig;
        }

        public async Task<bool> IsCurrentActiveToken() => await IsActiveAsync(GetCurrentAsync());

        public async Task DeactivateCurrentAsync() => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token) => await _cache.GetStringAsync(GetKey(token)) == null;

        public async Task DeactivateAsync(string token)
        {
            await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(2)
                });
        }

        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token) => $"tokens:{token}:deactivated";
    }
}
