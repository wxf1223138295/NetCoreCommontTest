using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecorateTest.Repository
{
    public class ShawnRepository: IShawnRepository
    {
        public string GetId()
        {
            return "ShawnId";
        }
    }

    public interface IShawnRepository
    {
        string GetId();
    }
}
