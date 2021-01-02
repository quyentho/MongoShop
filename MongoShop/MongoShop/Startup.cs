using System;
using System.Reflection;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoShop.BusinessDomain;
using MongoShop.BusinessDomain.Carts;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using MongoShop.BusinessDomain.Wishlists;
using MongoShop.Services.FileUpload;

namespace MongoShop
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
            services.AddControllersWithViews();

            // requires using Microsoft.Extensions.Options
            services.Configure<DatabaseSetting>(
                  Configuration.GetSection(nameof(DatabaseSetting)));

            services.AddSingleton<IDatabaseSetting>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSetting>>().Value);

            services.AddScoped<IUserConfirmation<ApplicationUser>, UserConfirmation>();

            var mongoDbIdentityConfiguration = new MongoDbIdentityConfiguration
            {
                MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = Configuration["DatabaseSetting:ConnectionString"],
                    DatabaseName = Configuration["DatabaseSetting:DatabaseName"]
                },
                IdentityOptionsAction = options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                    options.Lockout.MaxFailedAccessAttempts = 3;

                    // ApplicationUser settings
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedAccount = true;
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";
                }
            };
            services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, Guid>(mongoDbIdentityConfiguration);

            services.AddSingleton<IProductServices, ProductServices>();

            services.AddSingleton<IUserServices, UserServices>();

            services.AddSingleton<IOrderServices,OrderServices>();

            services.AddSingleton<ICategoryServices, CategoryServices>();
            services.AddSingleton<ICartServices, CartServices>();
            services.AddSingleton<IWishlistServices, WishlistServices>();
            services.AddSingleton<IOrderServices, OrderServices>();

            services.AddTransient<IFileUploadService, FileUploadService>();

            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "admin_route",
                    areaName: "Admin",
                    pattern: "Admin/{controller=product}/{action=index}/{id?}"
                    );

                endpoints.MapControllerRoute("default_route", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
