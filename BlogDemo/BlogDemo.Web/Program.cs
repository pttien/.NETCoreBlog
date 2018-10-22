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
                    userMgr.CreateAsync(new ApplicationUser { UserName = "admin", Email = "admin@us.com" }, "Admin@pass1");
                    userMgr.CreateAsync(new ApplicationUser { UserName = "member", Email = "demo@us.com" }, "Demo@pass1");
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
