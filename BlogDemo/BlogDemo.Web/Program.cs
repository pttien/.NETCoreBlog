using System.Collections.Generic;
using System.Linq;
using Blogdemo.Services;
using BlogDemo.Domain.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogDemo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                try
                {
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                }
                catch { }

                var userMgr = (UserManager<ApplicationUser>)services.GetRequiredService(typeof(UserManager<ApplicationUser>));
                
                if (!userMgr.Users.Any())
                {
                    string[] roleNames = { "Admin", "Author", "Member" };
                    var RoleManager = (RoleManager<IdentityRole>)services.GetRequiredService(typeof(RoleManager<IdentityRole>));

                    foreach (var roleName in roleNames)
                    {       //create the roles and seed them to the database: Question 1
                        RoleManager.CreateAsync(new IdentityRole(roleName));
                    }

                    var admin = new ApplicationUser { UserName = "admin", Email = "admin@us.com" };
                        userMgr.CreateAsync(admin, "Admin@pass1");
                    userMgr.AddToRolesAsync(admin, roleNames);

                    var author = new ApplicationUser { UserName = "author", Email = "demo@us.com" };
                    userMgr.CreateAsync(author, "Author@pass1");
                    userMgr.AddToRolesAsync(author, new List<string> { "Author", "Member" });
                }

                if (!context.Posts.Any())
                {
                    services.GetRequiredService<IStorageService>().Reset();
                    AppData.Seed(context);
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
