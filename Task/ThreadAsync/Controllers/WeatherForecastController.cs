using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ThreadAsync.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        private static string ReturnValue()
        {
            return "";
        }
        [HttpGet]
        public async Task<string> Get()
        {
            AsyncLocal<string> asyncLocal=new AsyncLocal<string>(); 

            

            ThreadLocal<string> user = new ThreadLocal<string>(ReturnValue, true);
  
            await Task.Run(() =>
            {
                Thread.Sleep(4000);
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:3");
                user.Value = $"{Thread.CurrentThread.ManagedThreadId}:3";
            });

            await Task.Run(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:4");
                user.Value = $"{Thread.CurrentThread.ManagedThreadId}:4";
            });

            await Task.Run(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine(user.Value = $"{Thread.CurrentThread.ManagedThreadId}:6");
                user.Value = $"{Thread.CurrentThread.ManagedThreadId}:6";
            });

            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine(user.Value = $"{Thread.CurrentThread.ManagedThreadId}:8");
                user.Value = $"{Thread.CurrentThread.ManagedThreadId}:8";
            });

            user.Value = $"{Thread.CurrentThread.ManagedThreadId}:5";

            var tst=user.Value;

           
                var tt = user.Values;

            return string.Empty;
        }
    }
}
