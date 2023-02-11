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
using Web2023_BE.ApplicationCore;
using Web2023_BE.ApplicationCore.Exceptions;

namespace Web2023_BE.HostBase
{
    /// <summary>
    /// Middleware thiết lập thông tin đầy đủ cho một request
    /// </summary>
    public class NormalMiddleware
    {
        private readonly ILogger _log;
        private readonly RequestDelegate _next;

        // Must have constructor with this signature, otherwise exception at run time
        public NormalMiddleware(
            ILogger<NormalMiddleware> log,
            RequestDelegate next)
        {
            _log = log;
            _next = next;
        }

        public async Task Invoke(HttpContext context, IContextService contextService)
        {
            try
            {
                await _next(context);
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
    }

    public static class SetNormalExtensions
    {
        /// <summary>
        /// Thiết lập thông tin user/tenant đang đăng nhập cho request hiện tại
        /// </summary>
        public static IApplicationBuilder UseSetNormal(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NormalMiddleware>();
        }
    }
}
