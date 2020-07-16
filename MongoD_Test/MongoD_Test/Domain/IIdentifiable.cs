using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoD_Test.Domain
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}
