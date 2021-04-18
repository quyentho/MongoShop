using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;

namespace MongoShop.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
                var builder = WebAssemblyHostBuilder.CreateDefault(args);
                builder.RootComponents.Add<App>("app");
                builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

            builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MapperProfile)));
            builder.Services.AddScoped(sp =>
                new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:5001")
                });
            await builder.Build().RunAsync();
        }
    }
}
