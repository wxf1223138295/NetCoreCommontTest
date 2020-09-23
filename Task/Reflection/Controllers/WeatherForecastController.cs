using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Reflection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IServiceProvider serviceProvider, ILogger<WeatherForecastController> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            var types = typeof(TestRef);

            var obj=_serviceProvider.GetService(types);

            var tt = typeof(IIntertest);

            var  tts=tt.GetMethod("Test").Invoke(obj,null);

            return tts.ToString();
        }
    }

    public class TestRef:IIntertest
    {
        public string Test()
        {
            return "sss";
        }
    }
}