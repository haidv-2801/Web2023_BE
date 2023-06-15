using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MISA.Legder.Domain.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;

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
            var accountID = jwtUtils.ValidateJwtToken(token);

#if DEBUG
            await _next(context);
#else

            if (string.IsNullOrWhiteSpace(accountID))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            context.Items["AccountID"] = accountID;
            await _next(context);
#endif
        }
    }
}
