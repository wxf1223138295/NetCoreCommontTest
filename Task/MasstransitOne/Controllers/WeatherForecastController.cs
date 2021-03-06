﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MasstransitOne.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IBusControl _bus;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IBusControl bus, ILogger<WeatherForecastController> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            _bus
            return "s";
        }
    }
}
