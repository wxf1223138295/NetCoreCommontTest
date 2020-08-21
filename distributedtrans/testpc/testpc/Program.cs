using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using testpc.Entity;

namespace testpc
{
    class Program
    {
        static void Main(string[] args)
        {
           

           var ihost= Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<ShawnDbContext>(
                        options => options.UseNpgsql(
                            "host=172.16.0.20;database=bank_cqm_test;username=cqm_app;password=SynyiCqm;PersistSecurityInfo=true;Maximum Pool Size=100;Connection Idle Lifetime=5;Connection Pruning Interval=5"));
                }).Build();

           IServiceProvider provider = ihost.Services;


           ihost.Start();


            using (var scope= provider.CreateScope())
            {
               var tt= scope.ServiceProvider.GetRequiredService<ShawnDbContext>();

               using (var trans=tt.Database.BeginTransaction())
               {
                   try
                   {
                       Table1 t1=new Table1
                       {
                           Id = 1,
                           Name = "Shawn1",
                           Score = 99
                       };

                       Table2 t2 = new Table2
                       {
                           Id = 1,
                           Name = "Shawn1",
                           Age =29
                       };

                       tt.Add(t1);

                       tt.Add(t2);

                       tt.SaveChanges();

                     trans.Commit();

                    }
                   catch (Exception e)
                   {
                       trans.Rollback();
                    }

               }

            }

            



            Console.WriteLine("Hello World!");
        }
    }
}
