using BlogDemo.Domain.Data;
using BlogDemo.Domain.Repositories;
using System;

namespace Blogdemo.Services
{
    public interface IDataService : IDisposable
    {
        IPostRepository Posts { get; }
        IAuthorRepository Authors { get; }
        void Complete();
    }

    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _db;

        public DataService(ApplicationDbContext db)
        {
            _db = db;

            Posts = new PostRepository(_db);
            Authors = new AuthorRepository(db);
        }

        public IPostRepository Posts { get; private set; }

        public IAuthorRepository Authors { get; private set; }

        public void Complete()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}