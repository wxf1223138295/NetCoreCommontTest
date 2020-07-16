using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoD_Test.Application.Order.Input;
using MongoD_Test.Context;
using MongoD_Test.Domain;
using MongoD_Test.Repository;

namespace MongoD_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMongoRepository<Order> _mongoRepository;
        private readonly IBookRepository _bookRepository;

        public OrderController(IMongoRepository<Order> mongoRepository, IBookRepository bookRepository)
        {
            _mongoRepository = mongoRepository;
            _bookRepository = bookRepository;
        }
        [HttpPost("OrderShip")]
        public async Task<ActionResult> OrderShip(CreateOrderInput input)
        {
            Order order = new Order
            {
                OrderDesc = input.OrderDesc,
                OrderNo = input.OrderNo,
                Amount = input.Amount,
                State = input.State
            };

            await _mongoRepository.AddAsync(order);

            return Ok();
        }

        [HttpPost("BookShip")]
        public async Task<ActionResult> BookShip(CreateBookInput input)
        {
            Book order = new Book
            {
                BookNo = input.BookNo,
                Author = input.Author,
                BookName = input.BookName,
                Category = input.Category,
                Price = input.Price
            };

            await _bookRepository.AddAsync(order);

            return Ok();
        }
    }
}
