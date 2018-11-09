using BlogDemo.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogDemo.Test
{
    public static class MockDbContext
    {
        public static ApplicationDbContext GetInstance(string databaseName = null)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(string.IsNullOrEmpty(databaseName) ? Guid.NewGuid().ToString() : databaseName)
                .EnableSensitiveDataLogging()
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
