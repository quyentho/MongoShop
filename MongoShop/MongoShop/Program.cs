using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoShop.BusinessDomain.Users;

namespace MongoShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build();

            //Seed database
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
									
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
										if (!roleManager.RoleExistsAsync(UserRole.Admin).Result)
										{
										    ApplicationRole role = new ApplicationRole();
												role.Name = UserRole.Admin;
												IdentityResult roleResult = roleManager.CreateAsync(role).Result;
										}
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    if(userManager.FindByEmailAsync("admin@mongoshop.com").GetAwaiter().GetResult() is null)
                    {

                        var user = new ApplicationUser { UserName = "admin@mongoshop.com", Email = "admin@mongoshop.com", Status = true };

                        userManager.CreateAsync(user, "1234567").GetAwaiter().GetResult();
                        userManager.AddToRoleAsync(user, UserRole.Admin).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Seeding DB failed", ex);
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
