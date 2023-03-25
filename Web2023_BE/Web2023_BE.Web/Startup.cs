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
using Microsoft.Owin.Cors;
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

namespace Web2023_BE.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDirectoryBrowser();

            //inject contact service
            HostBaseFactory.InjectContextService(services, Configuration);

            //cache
            HostBaseFactory.InjectCached(services, Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowCROSPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                    });
            });


            services.AddHttpContextAccessor();


            services.AddControllersWithViews(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // Config authenication
            HostBaseFactory.InjectJwt(services, Configuration);
            IdentityModelEventSource.ShowPII = true;
            services.AddMvc(x => x.EnableEndpointRouting = false);

            //
            services.AddScoped<ClientIpCheckActionFilter>(container =>
            {
                var loggerFactory = container.GetRequiredService<ILoggerFactory>();
                var cache = container.GetRequiredService<IMemoryCache>();
                var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

                return new ClientIpCheckActionFilter(
                    Configuration["AdminSafeList"], cache, logger);
            });

            //File storage
            HostBaseFactory.InjectStorageService(services, Configuration);
           
            //Add Elasticsearch
            //services.AddElasticsearch(Configuration);
            var url = Configuration["elasticsearch:url"];
            var settings = new ConnectionSettings(new Uri(url)).DefaultIndex("posts");
            var client = new ElasticClient(settings);
            services.AddSingleton(client);

          

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();

            //base
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            //account
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            //post
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostService, PostService>();

            //menu
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuService, MenuService>();

            //book
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookService, BookService>();

            //elastic search
            services.AddScoped(typeof(IElasticRepository<>), typeof(ElasticRepository<>));
            services.AddScoped(typeof(IElasticService<>), typeof(ElasticService<>));

            //book order
            services.AddScoped<IBookOrderRepository, BookOrderRepository>();
            services.AddScoped<IBookOrderService, BookOrderService>();

            //safe address
            services.AddScoped<ISafeAddressRepository, SafeAddressRepository>();
            services.AddScoped<ISafeAddressService, SafeAddressService>();

            //role address
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            //library card
            services.AddScoped<IContactSubmitRepository, ContactSubmitRepository>();
            services.AddScoped<IContactSubmitService, ContactSubmitService>();

            //carousel
            services.AddScoped<ICarouselService, CarouselService>();

            //partner
            services.AddScoped<IPartnerService, PartnerService>();

            //footer
            services.AddScoped<IHtmlSectionService, HtmlSectionService>();

            //teachintro
            services.AddScoped<ITechIntroService, TechIntroService>();

            // folder
            services.AddScoped<IFolderService, FolderService>();

            //image
            services.AddScoped<IImageManagerService, ImageManagerService>();

            

          

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICorsService corsService, Microsoft.AspNetCore.Cors.Infrastructure.ICorsPolicyProvider corsPolicyProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ErrorHandlingMiddleWare>();

            //app.UseMiddleware<JwtMiddleware>();

            app.UseRouting();


            // Set up custom content types - associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".myapp"] = "application/x-msdownload";
            provider.Mappings[".htm3"] = "text/html";
            provider.Mappings[".image"] = "image/png";
            // Replace an existing mapping
            provider.Mappings[".rtf"] = "application/x-msdownload";
            // Remove MP4 videos.
            provider.Mappings.Remove(".mp4");

            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                FileProvider = new PhysicalFileProvider(
           Path.Combine(env.ContentRootPath, "Stores")),
                RequestPath = "/stores",
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 86400;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;

                    ctx.Context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = "*";
                    ctx.Context.Response.Headers[HeaderNames.AccessControlMaxAge] = durationInSeconds.ToString();
                    ctx.Context.Response.Headers[HeaderNames.Vary] = "Accept-Encoding";
                },
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Images")),
                RequestPath = "/Images"
            });



            // using Microsoft.Extensions.FileProviders;
            // using System.IO;
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Stores")),
                RequestPath = "/stores",
                EnableDirectoryBrowsing = true
            });


            app.UseStaticFiles(); // For the wwwroot folder.
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMiddleware<AdminSafeListMiddleware>(Configuration["AdminSafeList"]);
        }


        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}
