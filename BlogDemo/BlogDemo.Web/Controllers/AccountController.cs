using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogdemo.Services;
using BlogDemo.Domain.Data;
using BlogDemo.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogDemo.Web.Controllers
{
    public class AccountController : Controller
    {

        #region " Fields & Constructor "

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDataService dataService;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDataService dataService
           )
        {
            this.dataService = dataService;
            this.userManager = userManager;
            this.signInManager = signInManager;
         
        }

        #endregion

        #region " Login / Logout / Access Denied "

        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            @ViewBag.Title = "Login";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, [FromQuery] string returnUrl="")
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var result = await this.signInManager.PasswordSignInAsync(
                model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
               
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    var author = await dataService.Authors.GetItem(e => e.AppUserName.ToLower() == model.UserName.ToLower());
                    if (author != null)
                    {

                        return Redirect("/Admin/Posts");
                    }
                    else
                    {
                        ViewData["IsAuthor"] = "false";
                        return Redirect("/");
                    }
                }                
                    
            }
             

            ModelState.AddModelError(string.Empty, "Login Failed");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion

        #region " Register "

        public IActionResult Register()
        {
            @ViewBag.Title = "Register";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email              
            };

            var result = await this.userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Member");
            }
            if (result.Succeeded)
            {
                return Redirect("~/Account/login");
            }

            return View(model);
        }

        #endregion

    }
}