using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogdemo.Services;
using BlogDemo.Domain.Data;
using BlogDemo.Domain.Helpers;
using BlogDemo.Domain.Models;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Configuration;
namespace BlogDemo.Web.Controllers
{
    public class PostController : Controller
    {
        IDataService _db;
        SignInManager<ApplicationUser> _sm;
        private readonly ICompositeViewEngine _viewEngine;
        public IConfiguration Configuration { get; }
        private UserManager<ApplicationUser> _userManager;
        public PostController(IDataService db, SignInManager<ApplicationUser> sm, ICompositeViewEngine viewEngine, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _sm = sm;
            _viewEngine = viewEngine;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1, string term = "")
        {
            var pager = new Pager(page, 10);
            IEnumerable<PostItem> posts;

            if (string.IsNullOrEmpty(term))
            {
                posts = await _db.Posts.GetList(p => p.Published > DateTime.MinValue, pager);
            }
            else
            {
                posts = await _db.Posts.Search(pager, term);
            }

            if (pager.ShowOlder) pager.LinkToOlder = $"blog?page={pager.Older}";
            if (pager.ShowNewer) pager.LinkToNewer = $"blog?page={pager.Newer}";

            var model = new ListModel
            {
                PostListType = PostListType.Blog,
                Posts = posts,
                Pager = pager
            };


            if (!string.IsNullOrEmpty(term))
            {
                ViewBag.Title = term;
                ViewBag.Description = "";
                model.PostListType = PostListType.Search;
            }
            ViewBag.Title = "Home";
            ViewBag.Cover = $"{Url.Content("~/")}{"data/admin/cover-desk.jpg"}";
            return View(model);
        }

        [Route("posts/{slug}")]
        public async Task<IActionResult> PostDetail(string slug)
        {
            try
            {
                var model = await _db.Posts.GetModel(slug);
                model.Post.Content = Markdown.ToHtml(model.Post.Content);

                ViewBag.Cover = string.IsNullOrEmpty(model.Post.Cover) ?
                    $"{Url.Content("~/")}{"data/default/cover-blog.png"}" :
                    $"{Url.Content("~/")}{model.Post.Cover}";
                ViewBag.Title = model.Post.Title;
                ViewBag.Description = model.Post.Description;

                return View(model);
            }
            catch
            {
                return Redirect("~/error/404");
            }

        }

        [Route("authors/{name}")]
        public async Task<IActionResult> Authors(string name, int page = 1)
        {
            var author = await _db.Authors.GetItem(a => a.AppUserName == name);

            var pager = new Pager(page);
            var posts = await _db.Posts.GetList(p => p.Published > DateTime.MinValue && p.AuthorId == author.Id, pager);

            if (pager.ShowOlder) pager.LinkToOlder = $"authors/{name}?page={pager.Older}";
            if (pager.ShowNewer) pager.LinkToNewer = $"authors/{name}?page={pager.Newer}";

            var model = new ListModel
            {
                PostListType = PostListType.Author,
                Author = author,
                Posts = posts,
                Pager = pager
            };
            ViewBag.Title = name;
            ViewBag.Cover = $"{Url.Content("~/")}{"data/admin/cover-desk.jpg"}";
            ViewBag.Description = "";

            return View("~/Views/Post/Index.cshtml", model);
        }

        [Route("categories/{name}")]
        public async Task<IActionResult> Categories(string name, int page = 1)
        {
            var pager = new Pager(page);
            var posts = await _db.Posts.GetListByCategory(name, pager);

            if (pager.ShowOlder) pager.LinkToOlder = $"categories/{name}?page={pager.Older}";
            if (pager.ShowNewer) pager.LinkToNewer = $"categories/{name}?page={pager.Newer}";

            var model = new ListModel
            {
                PostListType = PostListType.Category,
                Posts = posts,
                Pager = pager
            };
            ViewBag.Title = name;
            ViewBag.Category = name;
            ViewBag.Description = "";
            ViewBag.Cover = $"{Url.Content("~/")}{"data/admin/cover-desk.jpg"}";
            ViewBag.Description = "";

            return View("~/Views/Post/Index.cshtml", model);
        }

        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            var viewName = $"~/Views/_Error.cshtml";
            var result = _viewEngine.GetView("", viewName, false);
            return View(viewName, code);
        }

        [Authorize]
        [Route("post/addcomment/{postId}/{comment}")]
        public async Task<IActionResult> AddComment(int postId, string comment)
        {
            var currentUserId = _userManager.GetUserId(User);
            await  _db.Posts.AddCommentToPost(postId, comment, currentUserId);
            var model = await _db.Posts.GetItem(e => e.Id == postId);

            return PartialView("_Comment", model.Comments);
        }
    }
}