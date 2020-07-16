using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoD_Test.Domain;
using MongoDB.Driver;

namespace MongoD_Test.Context
{
    public class BookRepository : IBookRepository
    {
        protected IMongoCollection<Book> Collection { get; }

        public BookRepository(IMongoDatabase database, string collectionName)
        {
            Collection = database.GetCollection<Book>(collectionName);
        }
        public async Task<Book> GetAsync(string bookno)
        {
            var result = await Collection.FindAsync<Book>(book => book.BookNo == bookno);
            return await result.FirstOrDefaultAsync();
        }
        public async Task AddAsync(Book entity)
            => await Collection.InsertOneAsync(entity);
    }
}
