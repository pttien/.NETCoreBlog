using Blogdemo.Services;
using BlogDemo.Domain.Data;
using BlogDemo.Domain.Helpers;
using BlogDemo.Domain.Models;
using BlogDemo.Web.Pages.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlogDemo.Web.Pages.Admin.Posts
{
    [Authorize]
    public class IndexModel : AdminPageModel
    {
        [BindProperty]
        public IEnumerable<PostItem> Posts { get; set; }
        public Pager Pager { get; set; }

        IDataService _db;

        public IndexModel(IDataService db)
        {
            _db = db;         
            Pager = new Pager(1);
        }

        public async Task<IActionResult> OnGetAsync(int pg = 1, string status = "A")
        {
            var author = await _db.Authors.GetItem(a => a.AppUserName == User.Identity.Name);
            IsAdmin = author.IsAdmin;

            Expression<Func<Post, bool>> predicate = p => p.Id > 0;
            Pager = new Pager(pg);

            if (IsAdmin)
            {
                if(status == "P")
                    predicate = p => p.Published > DateTime.MinValue;
                else if(status == "D")
                    predicate = p => p.Published == DateTime.MinValue;
            }
            else
            {
                if (status == "P")
                    predicate = p => p.Published > DateTime.MinValue && p.AuthorId == author.Id;
                else if (status == "D")
                    predicate = p => p.Published == DateTime.MinValue && p.AuthorId == author.Id;
                else
                    predicate = p => p.AuthorId == author.Id;
            }

            Posts = await _db.Posts.GetList(predicate, Pager);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var author = await _db.Authors.GetItem(a => a.AppUserName == User.Identity.Name);
            IsAdmin = author.IsAdmin;

            var page = int.Parse(Request.Form["page"]);
            var term = Request.Form["search"];

            Pager = new Pager(page);

            if(IsAdmin)
                Posts = await _db.Posts.Search(Pager, term);
            else
                Posts = await _db.Posts.Search(Pager, term, author.Id);

            return Page();
        }
    }
}