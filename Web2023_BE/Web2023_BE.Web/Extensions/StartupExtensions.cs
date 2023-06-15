using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Web2023_BE.ApplicationCore.Interfaces.IServices;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.ApplicationCore.Services;
using Web2023_BE.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Web2023_BE.ApplicationCore.MiddleWare;

namespace Web2023_BE.ApplicationCore.Extensions
{
    public static class StartupExtensions 
    {
        public static void UseMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleWare>();
            //app.UseMiddleware<JwtMiddleware>();
            //app.UseMiddleware<AdminSafeListMiddleware>(Configuration["AdminSafeList"]);
        }

        public static void AddCors(this IServiceCollection services)
        {
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
        }

        public static void InjectDependencies(this IServiceCollection services)
        {
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
    }
}
