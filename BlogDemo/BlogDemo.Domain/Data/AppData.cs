using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogDemo.Domain.Data
{
    public class AppData
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Posts.Any())
                return;

            context.Authors.Add(new Author
            {
                AppUserName = "admin",
                Email = "admin@us.com",
                DisplayName = "Administrator",
                Avatar = "data/admin/avatar.png",
                Bio = "<p>Something about <b>administrator</b></p>",
                IsAdmin = true,
                Created = DateTime.UtcNow.AddDays(-120)
            });

            context.Authors.Add(new Author
            {
                AppUserName = "author",
                Email = "member@us.com",
                DisplayName = "Demo user",
                Bio = "Short description about this user and blog.",
                Avatar = "data/member/Avatar.jpg",
                Created = DateTime.UtcNow.AddDays(-110)
            });

            context.SaveChanges();
            var adminId = context.Authors.Single(a => a.AppUserName == "admin").Id;
            var demoId = context.Authors.Single(a => a.AppUserName == "author").Id;

            context.Posts.Add(new Post
            {
                Title = "How to get started learning Sitecore",
                Slug = "How-to-get-started-learning-Sitecore",
                Description = SeedData.FeaturedDesc,
                Content = SeedData.PostWhatIs,
                Categories = "welcome,blog",
                AuthorId = adminId,
                Cover = "data/admin/cover-blog.png",
                PostViews = 5,
                Rating = 4.5,
                IsFeatured = true,
                Published = DateTime.UtcNow.AddDays(-100)
            });

            context.Posts.Add(new Post
            {
                Title = "How to inject the SqlShell admin tool to Sitecore 7",
                Slug = "How-to-inject-the-SqlShell-admin-tool-to-Sitecore-7",
                Description = "Sometimes, we want to execute the SQL script to get insight into the current status of our Sitecore instance. It will be easy peasy lemon squeezy if you can access the server database.",
                Content = SeedData.PostFeatures,
                Categories = "blog",
                AuthorId = adminId,
                Cover = "data/admin/ss_7.jpeg",
                PostViews = 15,
                Rating = 4.0,
                Published = DateTime.UtcNow.AddDays(-55)
            });

            context.Posts.Add(new Post
            {
                Title = "Fabletics x Girl Up Meet the Girl Up leaders from Fabletics new collection!",
                Slug = "fabletics-x-girl-up-meet-the-girl-up-leaders-from-fabletics-new-collection",
                Description = "Fabletics’ new #GirlAlmighty capsule collection benefiting Girl Up,  is inspired by Kate Hudson’s vision for Fabletics to be a fashion-focused activewear brand with the mission to empower women by making a healthy, active lifestyle accessible to everyone. The new collection inspires girls and women to stand up and celebrate each other regardless of size, shape, age or ability.",
                Content = SeedData.PostDemo,
                AuthorId = demoId,
                Cover = "data/member/2018/10/Fabletics-2-Sized.png",
                PostViews = 25,
                Rating = 3.5,
                Categories = "Girl",
                Published = DateTime.UtcNow.AddDays(-10)
            });



            context.SaveChanges();
        }
    }

    public class SeedData
    {
        public static readonly string FeaturedDesc = @"Personally, I’d like to summarize the highlight Sitecore learning resources for myself and for everyone. I’m so happy that the Sitecore community is awesome and there are many helpful Sitecore stuffs are kept created daily.";

        public static readonly string PostWhatIs = @"Personally, I’d like to summarize the highlight Sitecore learning resources for myself and for everyone. I’m so happy that the Sitecore community is awesome and there are many helpful Sitecore stuffs are kept created daily.

    ![file-mgr.png](/data/admin/ls_1.jpg)
    Sitecore Knowledge Base
    Sitecore Experience Platform (XP): https://sitecore.stackexchange.com/questions/1737/how-can-i-get-started-learning-sitecore great Q&A on Sitecore Stack Exchange by Sitecore MVP Mark Cassidy
    Note 1: Sitecore XP 9 step-by-step install guide on your machine
    version 9.0.0
    version 9.0.1
    version 9.0.2
    Note 2: how to reduce your Sitecore recycle time after a build on your machine
    version 9: by Nick Wisselman
    version 8: by Kam Figy
    Note 3: powerful Sitecore Tools and Modules to make your life easier by Amitabh Vyas 
    Sitecore Experience Commerce (XC): https://sitecore.stackexchange.com/questions/12771/how-to-start-development-with-sitecore-commerce-9-sitecore-commerce-9-learning great Q&A on Sitecore Stack Exchange by Peter Prochazka
    Note: Sitecore XC 9 step-by-step install guide on your machine
    version 9.0.0
    version 9.0.1
    version 9.0.2

Sitecore References
Highly recommend everyone to set the following Sitecore demo(s) up on your own machine so that we can have a chance to learn something for our own implementation:
![file-mgr.png](/data/admin/ls_4.jpg)

Habitat: it is an example Sitecore solution built on the Helix architecture principles. It is designed to show how a Helix-based solution can be architected, and to demonstrate how tooling can be used to accomplish publishing, serialization, and testing. Habitat is not intended to be a starter solution, or as a recommendation of tools for your solutions
GitHub: https://github.com/Sitecore/Habitat/
Live site: N/A
Install guide: https://github.com/Sitecore/Habitat/wiki/01-Getting-Started
Habitat Home: HabitatHome Demo and the tools and processes in it is a Sitecore® solution example built using Sitecore Experience Accelerator™ (SXA) on Sitecore Experience Platform™ (XP) following the Helix architecture principles
GitHub: https://github.com/Sitecore/Sitecore.HabitatHome.Content
Live site: https://experienceplatform.habitathomedemo.com/
Step-by-step install guide on your machine:
Sitecore XP 9.0.2 and SXA 1.7.1
Sitecore XP 9.0.1 and SXA 1.6.0
Habitat Home Commerce: HabitatHome Commerce Demo and the tools and processes in it is a Sitecore® solution example built using Sitecore Experience Accelerator™ (SXA) on Sitecore Experience Platform™ (XP) and Sitecore Experience Commerce™ (XC) following the Helix architecture principles
GitHub: https://github.com/Sitecore/Sitecore.HabitatHome.Commerce
Live site: https://experiencecommerce.habitathomedemo.com/
Step-by-step install guide on your machine:
Sitecore XC 9.0.2
Sitecore XC 9.0.1


Sitecore Community Networks
There are many experienced Sitecore developers around the world and they’re willing to help you out (just a friendly reminder: playing around with our best friend Google so that you would have a good question / query before reaching them).

![file-mgr.png](/data/admin/ls_2.jpg)

Sitecore Slack (please submit the registration form)
Note: excellent Sitecore Slack Community Guidelines & Help by Sitecore MVP Kamruz Jaman
Sitecore Stack Exchange
Sitecore Community
Sitecore Linkedin Group
Sitecore Facebook Group
Happy Sitecore Learning!
";

        public static readonly string PostFeatures = @"Sometimes, we want to execute the SQL script to get insight into the current status of our Sitecore instance. It will be easy peasy lemon squeezy if you can access the server database.

![file-mgr.png](/data/admin/ss_6.jpeg)
Another way is to play around with SqlShell admin tool if you’re Sitecore admin + your Sitecore instance version is 8.1 Update 2 or later.

Our one is 7.2.0 and the Gatekeeper does NOT allow to access the database server. We have to go through the back-and-forth process to get the SQL query result so it takes quite much time and effort to do that.

Personally, I wanted to clear the obstacle above by injecting the admin tool SqlShell into our Sitecore 7 instance. It’s a tricky way so you should use it at your own risk 😀

![file-mgr.png](/data/admin/ss_7.jpeg)

How did I do that?
install a fresh Sitecore 8.1.2 instance so that I have a good chance to avoid compatibility issue of the latest versions
Note: should use SIM to install it very easily and quickly
ensure to enable SqlShell admin tool
create a Sitecore package so that I will be able to install it to Sitecore 7 later
Note 1: SqlShell is referring to Sitecore.ExperienceContentManagement.Administration.dll so we need to package that file as well
![file-mgr.png](/data/admin/ss_1.png)
![file-mgr.png](/data/admin/ss_2.png)

Note 2: you can download SqlShell_AdminTool812_InstallationWizard.zip if you’re so lazy like me 😀
install a fresh Sitecore 7.2.0 instance
Note: should use SIM to install it very easily and quickly
(optional) install SqlShell_AdminTool812_InstallationWizard.zip to Sitecore 7 instance via Installation Wizard
(optional) assume that the Gatekeeper does NOT allow to install Sitecore package so I need to download SqlShell_AdminTool812_FileExplorer.zip and then upload it to 
![file-mgr.png](/data/admin/dbi_6.png)

ensure to select Website folder in the content tree and then click Upload button to upload SqlShell_AdminTool812_FileExplorer.zip

![file-mgr.png](/data/admin/ss_4.png)

after clicking Next button, I would see the following dialog, ensure to check Unpack ZIP files on the server checkbox  and then click Upload > button to complete
![file-mgr.png](/data/admin/ss_5.png)


";

        public static readonly string PostDemo = @"
![data/member/2018/10/Fabletics-1-Sized.png](/data/member/2018/10/Fabletics-1-Sized.png)



Fabletics’ new #GirlAlmighty capsule collection benefiting Girl Up,  is inspired by Kate Hudson’s vision for Fabletics to be a fashion-focused activewear brand with the mission to empower women by making a healthy, active lifestyle accessible to everyone. The new collection inspires girls and women to stand up and celebrate each other regardless of size, shape, age or ability.

Members of Girl Up’s CSU Northridge campus chapter joined Kate Hudson for a #GirlAlmighty photo shoot featuring the collection in Rancho Palos Verdes, California – where the girls were able to model the fun styles. “I’m so excited to unveil the Girl Almighty collection in support of Girl Up. It has been a truly rewarding experience to work closely with the organization and the girls to create a collection that spreads the message that girls and women are the fearless leaders of tomorrow,” said co-founder of Fabletics, Kate Hudson.

We talked to the Girl Up girl leaders who took part in the shoot to hear their thoughts on the new collection!

![data/member/2018/10/Fabletics-2-Sized.png](/data/member/2018/10/Fabletics-2-Sized.png)

Angelica Wong, 20 (right)

Being able to work alongside Kate was such a blast; she was so friendly, humble and inviting– it was truly an incredible experience.

I am so excited for the #GirlAlmighty line because it showcases the importance of strong girls supporting each other. Each piece from this line is so cute and trendy, all your girlfriends will want to join the movement. Girls can be cute AND strong and I feel like that’s what this line is really about which is awesome! Everything in this line truly does make you feel like you can take on the world, so I can’t wait to see more girls rocking the #GirlAlmighty line.

The fact that this line supports body positivity and is truly celebrating girls standing up for each other means so much to me! Girl power starts within, if you feel good in the clothes you’re in, you’ll want to pass on that feeling and I feel like that is exactly what this line does. Face it, girls are strong and even stronger in numbers so we should focus on building each other up that way we can all reach our full potential. Plus, being able to do this all while wearing something that looks stunning and makes us feel amazing is awesome!

Alondra Serrano, 22 (middle back)

Kate was a great person all around! She was funny and made the shoot a very fun experience!

I am excited about the #GirlAlmighty line because that phrase on its own is so powerful and says so much! As well as benefiting the great cause of Girl Up! Plus just like all of Fabletics lines, #GirlAlmighty is full of very stylish and comfortable pieces!

#GirlAlmighty Fabletics line is so special! Promoting body positivity and overall girl empowerment! To me this is outstanding! Being a part of a sorority, I’ve learned that as girls we stand together and by doing so, you’ll always have endless support! When we work together we can become a voice for girls all around the world. It was a great honor to get to spread that through #GirlAlmighty!

Michelle Reyes, 22 (left)

Meeting Kate was the most amazing experience. She was so down to earth. Although I was initially star struck, she made me so comfortable around her.

I am excited about the #GirlAlmighty line because I think it will remind women that we are powerful. We are the future. So many people will doubt us, and we may even doubt ourselves, but we can achieve anything and everything.

To me, this means that we must remember that we are in this together. Women must unite in order to help others see that it is so important to just accept one and other for who we are. Individually unique, and together complete.";

    }
}
