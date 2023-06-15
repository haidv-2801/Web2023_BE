using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web2023_BE.ApplicationCore;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.ApplicationCore.MiddleWare;
using Web2023_BE.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Web2023_BE.ApplicationCore.Helpers;
using Nest;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Cors.Infrastructure;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Formatters;
using Web2023_BE.HostBase;
using Web2023_BE.ApplicationCore.Interfaces.IServices;
using Web2023_BE.ApplicationCore.Services;
using Web2023_BE.ApplicationCore.Authorization;
using Web2023_BE.ApplicationCore.Entities;
using Microsoft.IdentityModel.Logging;
using SixLabors.ImageSharp.Processing.Processors;
using Web2023_BE.ApplicationCore.Extensions;

namespace Web2023_BE.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDirectoryBrowser();

            var configuration = new ConfigurationBuilder()
           .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json")
           .Build();

            //================ inject base ================
            HostBaseFactory.InjectContextService(services, Configuration);
            HostBaseFactory.InjectCached(services, Configuration);
            HostBaseFactory.InjectJwt(services, Configuration);
            IdentityModelEventSource.ShowPII = true;
            HostBaseFactory.InjectStorageService(services, Configuration);
            services.AddSingleton(configuration);

            TinyMapperExtension.Bind();
            //================ inject base ================

            StartupExtensions.AddCors(services);
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddHttpContextAccessor();
            services.AddSwaggerGen();
            services.InjectDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICorsService corsService, Microsoft.AspNetCore.Cors.Infrastructure.ICorsPolicyProvider corsPolicyProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseMiddlewares();
            app.UseRouting();
            app.UseStaticFiles();
            app
               .UseCors(policy =>
                   policy
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials()
                       .WithOrigins("http://localhost:3000"));

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
