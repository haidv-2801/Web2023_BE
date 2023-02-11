using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Web2023_BE.Cache;
using Web2023_BE.ApplicationCore;
using Web2023_BE.ApplicationCore.Exceptions;
using Web2023_BE.Domain.Shared.Constants;
using Web2023_BE.Cache.Constants;

namespace Web2023_BE.HostBase
{
    /// <summary>
    /// Middleware thiết lập thông tin đầy đủ cho một request
    /// </summary>
    public class AuthContextHandlerMiddleware
    {
        private readonly ILogger _log;
        private readonly RequestDelegate _next;
        private readonly ICacheService _cacheService;

        // Must have constructor with this signature, otherwise exception at run time
        public AuthContextHandlerMiddleware(
            ILogger<AuthContextHandlerMiddleware> log,
            ICacheService cacheService,
            RequestDelegate next)
        {
            _log = log;
            _next = next;
            _cacheService = cacheService;
        }

        public async Task Invoke(HttpContext context, IContextService contextService)
        {
            try
            {
                var valid = await this.ProcessAuthenToken(context, contextService);
                if (valid)
                {
                    await _next(context);
                }
            }
            catch (BusinessException ex)
            {
                //lỗi nghiệp vụ ném ra -> ném về client mã 550
                context.Response.StatusCode = 550;
                await context.Response.WriteAsync(ex.GetClientReturn(), Encoding.UTF8);
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = 403;
                //Xử lý lỗi check quyền không có quyền mà can thiệp vào response trong BusinessAuthorizeFilter bị lỗi: StatusCode cannot be set because the response has already started.
                if (ex != null)
                {
                    await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(ex.Data), Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                //các lỗi khác -> ghi lỗi và trả message về client
                _log.LogError(ex, ex.Message);
                var msg = "Exception";
#if DEBUG
                msg = ex.ToString();
#endif
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(msg, Encoding.UTF8);
            }
        }

        /// <summary>
        /// Đọc thông tin authen token của request
        /// </summary>
        /// <remarks>false: request có vấn đề và phải dừng lại</remarks>
        private async Task<bool> ProcessAuthenToken(HttpContext context, IContextService contextService)
        {
            if (context.Request == null || context.Request.Headers == null || !context.Request.Headers.ContainsKey(HeaderKeys.Authorization))
            {
                return true;
            }

            string authHeader = context.Request.Headers[HeaderKeys.Authorization];
            if (!string.IsNullOrEmpty(authHeader))
            {
                string token = authHeader.Split(new char[] { ' ' })[1];
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);
                var payload = jsonToken.Payload;

                var contextData = new ContextData();
                contextData.UserId = int.Parse(payload[TokenKeys.UserID].ToString());
                contextData.DatabaseId = int.Parse(payload[TokenKeys.DatabaseID].ToString());
                contextData.SessionId = payload[TokenKeys.SessionId].ToString();

                //Kiểm tra phiên đăng nhập còn hợp lệ không
                if (context.Request.Cookies != null)
                {
                    const string key = CookieKeys.AuthSession;
                    if (context.Request.Cookies.ContainsKey(key))
                    {
                        var cookieSessionId = context.Request.Cookies[key];
                        if (cookieSessionId != contextData.SessionId)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync("Token diff Cookie", Encoding.UTF8);
                            return false;
                        }

                        //validate phiên đăng nhập còn hợp lệ không
                        if (!this.ValidateSession(contextData.SessionId))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync("Session not found", Encoding.UTF8);
                            return false;
                        }
                    }
                }

                contextService.SetContext(contextData);
            }

            return true;
        }

        /// <summary>
        /// Kiểm tra phiên làm việc còn hợp lệ không
        /// Đọc vào cache nếu còn thông tin context theo sessionId thì ok
        /// </summary>
        private bool ValidateSession(string sessionId)
        {
            var cacheParam = new CacheParam()
            {
                Name = CacheItemName.ContextData,
                SessionId = sessionId
            };
            var contextData = _cacheService.Get<ContextData>(cacheParam);
            return contextData != null;
        }
    }

    public static class SetAuthContextHandlerExtensions
    {
        /// <summary>
        /// Thiết lập thông tin user/tenant đang đăng nhập cho request hiện tại
        /// Để dùng món này phải inject ICacheService
        /// </summary>
        public static IApplicationBuilder UseSetAuthContextHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthContextHandlerMiddleware>();
        }
    }
}
