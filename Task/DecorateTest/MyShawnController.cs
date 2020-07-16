using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DecorateTest.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DecorateTest
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyShawnController : ControllerBase
    {
        private readonly IShawnRepository _repository;

        public MyShawnController(IShawnRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var re = _repository.GetId();
            return Ok(re);
        }
    }
}
