using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DecorateTest.Repository;

namespace DecorateTest.DecorateRepository
{
    public class CacheRepository:IShawnRepository
    {
        private IShawnRepository _repository;

        public CacheRepository(IShawnRepository repository)
        {
            _repository = repository;
        }
        public string GetId()
        {
            var ty=_repository.GetId();
            return "CacheId:"+ ty;
        }
    }
}
