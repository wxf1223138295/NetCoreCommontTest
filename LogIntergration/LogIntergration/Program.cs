using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Compact;

namespace LogIntergration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.ConfigureServices((context, service) =>
                    {
                        service.AddControllers();
                    });
                    
                    webBuilder.ConfigureAppConfiguration((context, config) =>
                        {
                            config.SetBasePath(Directory.GetCurrentDirectory());
                            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                            config.AddJsonFile(
                                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                                optional: true);
                            config.AddEnvironmentVariables();
                        });

                    webBuilder.UseSerilog(
                        (context, logconfig) =>
                        {
                            logconfig.Enrich.FromLogContext()
                                .ReadFrom.Configuration(context.Configuration, "MySerilog")
                                .Enrich.FromLogContext()
                                .WriteTo.Debug()
                                .WriteTo.Console(new RenderedCompactJsonFormatter());
                        });




                    //webBuilder.ConfigureLogging((context, logbuilder) =>
                    //{


                    //    logbuilder.SetMinimumLevel(LogLevel.Information);

                    //    logbuilder.AddSerilog();
                    //    logbuilder.AddConsole();

                    //});



                    webBuilder.Configure((context, app) =>
                    {
                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }

                        app.UseHttpsRedirection();

                        app.UseRouting();

                        app.UseAuthorization();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });

                });
    }
}
