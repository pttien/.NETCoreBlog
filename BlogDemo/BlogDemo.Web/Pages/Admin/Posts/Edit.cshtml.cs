using Blogdemo.Services;
using BlogDemo.Domain.Data;
using BlogDemo.Domain.Models;
using BlogDemo.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlogDemo.Web.Pages.Admin.Posts
{
    public class EditModel : AdminPageModel
    {
        [BindProperty]
        public PostItem PostItem { get; set; }

        IDataService _db;

        public EditModel(IDataService db )
        {
            _db = db;
        }

        public async Task OnGetAsync(int id)
        {
            var author = await _db.Authors.GetItem(a => a.AppUserName == User.Identity.Name);
            IsAdmin = author.IsAdmin;
            PostItem = new PostItem { Author = author, Cover = "data/admin/cover-desk.jpg" };

            if (id > 0)
                PostItem = await _db.Posts.GetItem(p => p.Id == id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            

            var user = await _db.Authors.GetItem(a => a.AppUserName == User.Identity.Name);
            IsAdmin = user.IsAdmin;

            PostItem.Author = await _db.Authors.GetItem(a => a.AppUserName == User.Identity.Name);
            

            if (ModelState.IsValid )
            {
                if (PostItem.Id > 0)
                { 
                    
                    var post = _db.Posts.Single(p => p.Id == PostItem.Id);
                    if(post != null)
                    {
                        PostItem.Author = await _db.Authors.GetItem(a => a.Id == post.AuthorId);
                    }
                }
                
                if (IsAdmin || _db.Authors.Single(a => a.Id == PostItem.Author.Id).AppUserName == User.Identity.Name)
                {
                    if (PostItem.Status == SaveStatus.Publishing)
                        PostItem.Published = DateTime.UtcNow;

                    if (PostItem.Status == SaveStatus.Unpublishing)
                        PostItem.Published = DateTime.MinValue;

                    
                    if (PostItem.Published == DateTime.Parse("1/1/2001"))
                        PostItem.Published = DateTime.MinValue;

                    PostItem.Slug = await GetSlug(PostItem.Id, PostItem.Title);

                    var item = await _db.Posts.SaveItem(PostItem);

                    PostItem = item;
                    Message = "Saved";

                    return Redirect($"~/Admin/Posts/Edit?id={PostItem.Id}");
                }
            }
            return Page();
        }

        public async Task<string> GetSlug(int id, string title)
        {
            string slug = title.ToSlug();
            Post post;

            if (id == 0)
                post = _db.Posts.Single(p => p.Slug == slug);
            else
                post = _db.Posts.Single(p => p.Slug == slug && p.Id != id);

            if (post == null)
                return await Task.FromResult(slug);

            for (int i = 2; i < 100; i++)
            {
                if (id == 0)
                    post = _db.Posts.Single(p => p.Slug == $"{slug}{i}");
                else
                    post = _db.Posts.Single(p => p.Slug == $"{slug}{i}" && p.Id != id);

                if (post == null)
                {
                    return await Task.FromResult(slug + i.ToString());
                }
            }

            return await Task.FromResult(slug);
        }
    }
}