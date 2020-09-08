using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DecorateTest2.MyScheduler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DecorateTest2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
    
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        private async Task<List<int>> Test()
        {
            return await Task.FromResult(new List<int>{1,2,3,4,5});
        }
        [HttpGet]
        public string Get(CancellationToken token)
        {
            List<int> list=new List<int>{1,2,3,4,5,6,7,8,9,10};


            LimitedConcurrencyLevelTaskScheduler scheduler=new LimitedConcurrencyLevelTaskScheduler(1);

            TaskFactory factory = new TaskFactory(scheduler);



            factory.StartNew(() =>
            {

                Thread.Sleep(5000);
                foreach (var i in list)
                {
                    if (i % 2 > 0)
                    {
                        Console.WriteLine($"count1:{i}");
                    }
                }
            },TaskCreationOptions.LongRunning);


            factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                foreach (var i in list)
                {
                    if (i % 2 > 0)
                    {
                        Console.WriteLine($"count2:{i}");
                    }
                }
            },  TaskCreationOptions.LongRunning);


            factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                foreach (var i in list)
                {
                    if (i % 2 > 0)
                    {
                        Console.WriteLine($"count3:{i}");
                    }
                }
            }, TaskCreationOptions.LongRunning);


            var taskid2=TaskScheduler.Default.Id;

            var taskid212 = TaskScheduler.Current.Id;

            var taskid22 = factory.Scheduler.Id;

            Console.WriteLine($"def:{taskid2} curr{taskid212}  fac {taskid22}");

            var tt = factory.Scheduler.MaximumConcurrencyLevel;

            Console.WriteLine($"MAX:{tt}");


            return "sdsdsds";
        }
    }
}
