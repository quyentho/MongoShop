using System;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MongoDB.Driver;
using MongoShop.BusinessDomain;
using MongoShop.BusinessDomain.Carts;
using MongoShop.BusinessDomain.Categories;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using MongoShop.BusinessDomain.Wishlists;
using MongoShop.ElasticSearch.Indexer;
using MongoShop.Infrastructure.Services.FileUpload;
using Nest;

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

            services.AddOptions();

            services.Configure<DatabaseSetting>(
                  Configuration.GetSection(nameof(DatabaseSetting)));

            services.AddSingleton<IMongoClient, MongoClient>((serviceProvider) =>
            {
                return new MongoClient(Configuration["DatabaseSetting:ConnectionString"]);
            });

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

            services.AddMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });

            services.AddMvc();

            services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, Guid>(mongoDbIdentityConfiguration);

            services.AddScoped<IProductServices, ProductServices>();

            services.AddScoped<IUserServices, UserServices>();

            services.AddScoped<IOrderServices, OrderServices>();

            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<ICartServices, CartServices>();
            services.AddScoped<IWishlistServices, WishlistServices>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IHomePageProductServices, HomePageProductServices>();
            services.AddScoped<IFileUploadService, FileUploadService>();

            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddMvc().AddNewtonsoftJson();

            services
                .AddFluentEmail("mongoshopemail@gmail.com")
                .AddRazorRenderer()
                .AddSmtpSender(new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("mongoshopemail@gmail.com", "passwordofmongoshopemail"),
                    EnableSsl = true
                });

            services.AddAuthentication()
              .AddGoogle(options =>
              {
                  options.ClientId = "140589299640-88d9fngq6s6ht88vpr1iktfl9fvnikgo.apps.googleusercontent.com";
                  options.ClientSecret = "j_6UG2HEEst7fZvc-YDgidqZ";
                  options.Events.OnRemoteFailure = (context) =>
                  {
                      context.Response.Redirect("/account/login");
                      context.HandleResponse();
                      return System.Threading.Tasks.Task.CompletedTask;
                  };
              })
            .AddFacebook(options =>
            {
                options.AppId = "745814422783717";
                options.AppSecret = "bd5da5bdfc0bd7e67fc2569aa96274c2";
                options.Events.OnRemoteFailure = (context) =>
                {
                    context.Response.Redirect("/account/login");
                    context.HandleResponse();
                    return System.Threading.Tasks.Task.CompletedTask;
                };
            });

            services.AddSingleton<IElasticClient>(ElasticSearchConfiguration.GetClient());

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseForwardedHeaders();
                //app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseForwardedHeaders();
            }

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "admin_route",
                    areaName: "Admin",
                    pattern: "Admin/{controller=product}/{action=index}/{id?}"
                    );

                endpoints.MapControllerRoute("default_route", "{controller=Customer}/{action=Index}/{id?}");
            });
        }
    }
}
