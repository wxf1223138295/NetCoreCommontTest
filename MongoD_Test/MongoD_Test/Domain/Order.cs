using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoD_Test.Domain
{
    public class Order: IIdentifiable
    {

        public Guid Id { get; set; }
        public string OrderNo { get; set; }
        public int State { get; set; }
        public decimal Amount { get; set; }
        public string OrderDesc { get; set; }
    }
}
