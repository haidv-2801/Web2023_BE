using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MISA.Legder.Domain.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Authorization;

namespace Web2023_BE.ApplicationCore.MiddleWare
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly AuthConfig _authConfig;
        public JwtMiddleware(RequestDelegate next, IOptions<AuthConfig> authConfig)
        {
            _next = next;
            _authConfig = authConfig.Value;
        }

        public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var role = jwtUtils.ValidateJwtToken(token);
            if (!string.IsNullOrEmpty(role))
            {
                // attach user to context on successful jwt validation
                context.Items["Role"] = role;
            }

            await _next(context);
        }
    }
}
