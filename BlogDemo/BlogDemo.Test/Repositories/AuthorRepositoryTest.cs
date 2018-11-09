using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogDemo.Domain.Data;
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
        public async Task Should_Return_All_Authors()
        {
            using (var dbContext = MockDbContext.GetInstance())
            {
                dbContext.SeedAuthor();
                var pager = new Pager(1, 10);
                var repo = new AuthorRepository(dbContext);
                var result = await repo.GetList(e => e.Id > 0, pager);

                result.Should().NotBeNull();
                result.Count().Should().Be(2);
            }

        }

        [Test]
        public async Task Should_Return_Null_When_Input_InValid_Keyword()
        {
            using (var dbContext = MockDbContext.GetInstance())
            {
                dbContext.SeedAuthor();
                var repo = new AuthorRepository(dbContext);
                var result = await repo.GetItem(e => e.AppUserName == "AppUserName6");

                result.Should().BeNull();

            }

        }

        [Test]
        public async Task Should_Return_An_Author_When_Input_Valid_Keyword()
        {
            using (var dbContext = MockDbContext.GetInstance())
            {
                dbContext.SeedAuthor();
                var repo = new AuthorRepository(dbContext);
                var result = await repo.GetItem(e => e.AppUserName == "AppUserName1");

                result.Should().NotBeNull();
                result.DisplayName.Should().Be("DisplayName1");
            }

        }

        [Test]
        public async Task Should_Change_Data_In_Existed_Author()
        {
            using (var dbContext = MockDbContext.GetInstance())
            {
                dbContext.SeedAuthor();
                var repo = new AuthorRepository(dbContext);
                var authorItem = repo.GetItem(e => e.AppUserName == "AppUserName1").Result;

                authorItem.DisplayName = "DisplayName Has been changed";

                await repo.Save(authorItem);

                var authorItemAfterChange = repo.GetItem(e => e.AppUserName == "AppUserName1").Result;

                authorItemAfterChange.DisplayName.Should().Be("DisplayName Has been changed");
            }

        }

        [Test]
        public async Task Should_Create_New_Author()
        {
            using (var dbContext = MockDbContext.GetInstance())
            {
                dbContext.SeedAuthor();
                var repo = new AuthorRepository(dbContext);
                var author = new Author
                {
                    AppUserName = "AppUserName3",
                    Email = "Email3@us.com",
                    DisplayName = "DisplayName3",
                    Avatar = "Avatar3",
                    Bio = "Bio3",
                    IsAdmin = true
                };

                await repo.Save(author);
                var pager = new Pager(1, 10);
                var result = await repo.GetList(e => e.Id > 0, pager);
                result.Count().Should().Be(3);
            }

        }

        [Test]
        public async Task Should_Remove_Author()
        {
            using (var dbContext = MockDbContext.GetInstance())
            {
                dbContext.SeedAuthor();
                var repo = new AuthorRepository(dbContext);
                await repo.Remove(1);
                var pager = new Pager(1, 10);
                var result = await repo.GetList(e => e.Id > 0, pager);
                result.Count().Should().Be(1);
            }

        }
    }
}
