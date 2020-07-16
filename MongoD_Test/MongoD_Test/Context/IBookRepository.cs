using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoD_Test.Domain;

namespace MongoD_Test.Context
{
    public interface IBookRepository
    {
        Task<Book> GetAsync(string bookno);
        Task AddAsync(Book entity);
    }
}
