using BlogDemo.Domain.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Test.Seeders
{
    public static class AuthorSeeder
    {
        public static void SeedAuthor(this ApplicationDbContext context)
        {
            context.Authors.Add(new Author
            {
                AppUserName = "AppUserName1",
                Email = "Email1@us.com",
                DisplayName = "DisplayName1",
                Avatar = "Avatar1",
                Bio = "Bio1",
                IsAdmin = true,
                Created = DateTime.UtcNow.AddDays(-120)
            });

            context.Authors.Add(new Author
            {
                AppUserName = "AppUserName2",
                Email = "Email2@us.com",
                DisplayName = "DisplayName2",
                Avatar = "Avatar2",
                Bio = "Bio2",
                IsAdmin = true,
                Created = DateTime.UtcNow.AddDays(-120)
            });

            context.SaveChanges();
        }
    }
}
