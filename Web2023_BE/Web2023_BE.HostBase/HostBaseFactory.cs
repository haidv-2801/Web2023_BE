
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web2023_BE.Cache;
using Web2023_BE.Domain.Shared.Debugs;
using Web2023_BE.ApplicationCore;
using Web2023_BE.Cache;
using Web2023_BE.Domain.Shared.Commons;
using Web2023_BE.Extension;
using Web2023_BE.ApplicationCore.FileSystem;
using Web2023_BE.Cache.Redis;
using Microsoft.Extensions.Caching.Redis;
using Web2023_BE.ApplicationCore.Entities;
using MISA.Legder.Domain.Configs;
using Web2023_BE.ApplicationCore.Authorization;
using Web2023_BE.ApplicationCore.Services;
using Web2023_BE.ApplicationCore.Interfaces.IServices;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace Web2023_BE.HostBase
{
    public static class HostBaseFactory
    {
        public static void InjectStopwatchService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStopwatchService, StopwatchService>();
        }

        public static void InjectContextService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IContextService, WebContextService>();
        }

        /// <summary>
        /// Khai báo khởi tạo service làm việc với cache
        /// </summary>
        public static void InjectCached(IServiceCollection services, IConfiguration configuration)
        {
            //mem cache
            //services.AddDistributedMemoryCache();
            services.AddMemoryCache();
            services.AddSingleton<IMemCached, MicrosoftMemCached>();

            //dist cache
            var distCacheds = GetRedisCached(configuration);
            services.AddSingleton(distCacheds);

            //service
            var config = ExtensionFactory.InjectConfig<CacheConfig>(configuration, "Cache", services);
            services.AddSingleton(config);
            services.AddSingleton<ICacheService, CacheService>();
        }

        public static void InjectJwt(IServiceCollection services, IConfiguration configuration)
        {
            var config = ExtensionFactory.InjectConfig<AuthConfig>(configuration, "Auth", services);
            services.AddSingleton(config);
            services.AddSingleton<IJwtUtils, JwtUtils>();
            services.AddSingleton<ITokenService, TokenService>();


            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JwtSettings.Key));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = false
            };
            // Config authenication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.SaveToken = true;
                  options.RequireHttpsMetadata = false;
                  options.TokenValidationParameters = tokenValidationParameters;
              });

        }




        private static Dictionary<string, IDistCached> GetRedisCached(IConfiguration configuration)
        {
            var result = new Dictionary<string, IDistCached>();
            var distConfig = ExtensionFactory.InjectConfig<Dictionary<string, RedisCacheConfig>>(configuration, "Cache:Redis");
            foreach (var item in distConfig)
            {
                var provider = new RedisCache(item.Value);
                var instance = new RedisCached(provider);
                result[item.Key] = instance;
            }

            return result;
        }


        ///// <summary>
        ///// Khai báo khởi tạo service làm việc với mem cache
        ///// </summary>
        //public static void InjectMicrosoftMemCached(IServiceCollection services, IConfiguration configuration)
        //{
        //    //services.AddDistributedMemoryCache();
        //    services.AddMemoryCache();
        //    services.AddSingleton<IMemCached, MicrosoftMemCached>();
        //}

        /// <summary>
        /// Khai báo service thao tác với file
        /// </summary>
        public static void InjectStorageService(IServiceCollection services, IConfiguration configuration)
        {
            var config = ExtensionFactory.InjectConfig<StorageConfig>(configuration, "Storage", services);

            services.AddSingleton<IStorageService, FileStorageService>();

        }

        /// <summary>
        /// Khai báo khởi tạo service mã hóa/giải mã
        /// </summary>
        public static void InjectEncodeService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEncodeService, EncodeService>();
        }
    }
}
