using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DecorateTest.DecorateRepository;
using DecorateTest.Repository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DecorateTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args);
        }

        public static async Task CreateHostBuilder(string[] args) =>
            await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddControllers();
                    services.AddMvc();
                    services.AddTransient<IShawnRepository, ShawnRepository>();
                    services.Decorate(typeof(IShawnRepository), typeof(CacheRepository));
                })
                .Configure(app =>
                {
                    app.UseHttpsRedirection();

                    app.UseRouting();

                    app.UseAuthorization();

                    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                })
                .Build()
                .RunAsync();


    }
}
