using System.Linq;
using AutoMapper;
using BlogDemo.Domain.Helpers;
using BlogDemo.Domain.Repositories;
using BlogDemo.Test.Seeders;
using FluentAssertions;
using NUnit.Framework;

namespace BlogDemo.Test.Repositories
{
    [TestFixture]
    public class AuthorRepositoryTest
    {
        [Test]
        public void Should_Return_All_Post()
        {
            using (var dbContext = MockDbContext.GetInstance())
            {
                dbContext.SeedAuthor();
                var pager = new Pager(1, 10);
                var repo = new AuthorRepository(dbContext);
                var result = repo.GetList(e => e.Id > 0, pager).Result;

                result.Should().NotBeNull();
                result.Count().Should().Be(2);
            }

        }

    }
}
