using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoD_Test.Context;
using MongoD_Test.Domain;
using MongoD_Test.Repository;
using MongoDB.Driver;

namespace MongoD_Test
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
            MongoCredential credential=MongoCredential.CreateCredential("shawndb","shawn","password");
            var server = new MongoServerAddress(host: "115.159.155.126", port: 27017);
            MongoClientSettings settings=new MongoClientSettings
            {
                Credential = credential,
                Server = server,
                ConnectionMode = ConnectionMode.Standalone
            };
            var mongoclient =new MongoClient(settings);

            services.AddSingleton(mongoclient);
            var databse = mongoclient.GetDatabase("shawndb");
            services.AddScoped<IMongoDatabase>((p) => { return databse; });
        

            services.AddScoped<IMongoRepository<Order>>(p =>
            {
                return new MongoRepository<Order>(databse, "Orders");
            });

            services.AddScoped<IBookRepository>(p =>
            {
                return new BookRepository(databse,"Books");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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
        }
    }
}
