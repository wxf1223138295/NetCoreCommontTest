using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoD_Test.Application.Order.Input
{
    public class CreateOrderInput
    {
        public string OrderNo { get; set; }
        public int State { get; set; }
        public decimal Amount { get; set; }
        public string OrderDesc { get; set; }
    }

    public class CreateBookInput
    {
        public string BookNo { get; set; }

        public string BookName { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }
    }
}
