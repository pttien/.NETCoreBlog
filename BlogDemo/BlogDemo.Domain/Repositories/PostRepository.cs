using AutoMapper;
using BlogDemo.Domain.Data;
using BlogDemo.Domain.Helpers;
using BlogDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlogDemo.Domain.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<PostItem>> GetList(Expression<Func<Post, bool>> predicate, Pager pager);
        Task<IEnumerable<PostItem>> GetListByCategory(string category, Pager pager);
        Task<IEnumerable<PostItem>> Search(Pager pager, string term, int author = 0);
        Task<PostItem> GetItem(Expression<Func<Post, bool>> predicate);
        Task<PostModel> GetModel(string slug);
        Task<PostItem> SaveItem(PostItem item);
        Task SaveCover(int postId, string asset);
    }

    public class PostRepository : Repository<Post>, IPostRepository
    {
        ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public PostRepository(ApplicationDbContext db, IMapper mapper) : base(db)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostItem>> GetList(Expression<Func<Post, bool>> predicate, Pager pager)
        {
            var skip = pager.CurrentPage * pager.ItemsPerPage - pager.ItemsPerPage;

            var drafts = _db.Posts.AsNoTracking()
                .Where(p => p.Published == DateTime.MinValue).Where(predicate)
                .Include(e=>e.Author)
                .OrderByDescending(p => p.Published).ToList();

            var pubs = _db.Posts.AsNoTracking()
                .Where(p => p.Published > DateTime.MinValue).Where(predicate)
                .Include(e => e.Author)
                .OrderByDescending(p => p.IsFeatured)
                .ThenByDescending(p => p.Published).ToList();

            var items = drafts.Concat(pubs).ToList();
            pager.Configure(items.Count);

            var postPage = items.Skip(skip).Take(pager.ItemsPerPage).ToList();

            return await Task.FromResult(_mapper.Map<List<PostItem>>(postPage));
        }

        public async Task<IEnumerable<PostItem>> GetListByCategory(string category, Pager pager)
        {
            var skip = pager.CurrentPage * pager.ItemsPerPage - pager.ItemsPerPage;

            var posts = _db.Posts.AsNoTracking()
                .Where(p => p.Published > DateTime.MinValue)
                .OrderByDescending(p => p.Published).ToList();

            var items = new List<Post>();

            foreach (var item in posts)
            {
                if (!string.IsNullOrEmpty(item.Categories))
                {
                    var cats = item.Categories.ToLower().Split(',');
                    if (cats.Contains(category.ToLower()))
                    {
                        items.Add(item);
                    }
                }
            }

            pager.Configure(items.Count);

            var postPage = items.Skip(skip).Take(pager.ItemsPerPage).ToList();

            return await Task.FromResult(_mapper.Map<List<PostItem>>(postPage));
        }

        // search always returns only published posts
        // for a search term and optional blog author
        public async Task<IEnumerable<PostItem>> Search(Pager pager, string term, int user = 0)
        {
            var skip = pager.CurrentPage * pager.ItemsPerPage - pager.ItemsPerPage;
            var results = new List<SearchResult>();
            var list = new List<PostItem>();

            IEnumerable<Post> posts;
            if (user == 0)
                posts = _db.Posts.AsNoTracking().Where(p => p.Published > DateTime.MinValue).ToList();
            else
                posts = _db.Posts.AsNoTracking().Where(p => p.Published > DateTime.MinValue && p.UserId == user.ToString()).ToList();

            foreach (var item in posts)
            {
                var rank = 0;
                var hits = 0;
                term = term.ToLower();

                if (item.Title.ToLower().Contains(term))
                {
                    hits = Regex.Matches(item.Title.ToLower(), term).Count;
                    rank += hits * 10;
                }
                if (item.Description.ToLower().Contains(term))
                {
                    hits = Regex.Matches(item.Description.ToLower(), term).Count;
                    rank += hits * 3;
                }
                if (item.Content.ToLower().Contains(term))
                {
                    rank += Regex.Matches(item.Content.ToLower(), term).Count;
                }

                if (rank > 0)
                {
                    results.Add(new SearchResult { Rank = rank, Item = _mapper.Map<PostItem>(item) });
                }
            }
            results = results.OrderByDescending(r => r.Rank).ToList();
            for (int i = 0; i < results.Count; i++)
            {
                list.Add(results[i].Item);
            }
            pager.Configure(list.Count);
            return await Task.Run(() => list.Skip(skip).Take(pager.ItemsPerPage).ToList());
        }

        public async Task<PostItem> GetItem(Expression<Func<Post, bool>> predicate)
        {
            var post = _db.Posts.AsNoTracking().Single(predicate);
            var item = _mapper.Map<PostItem>(post);

            item.Author.Avatar = string.IsNullOrEmpty(item.Author.Avatar) ? "lib/img/avatar.jpg" : item.Author.Avatar;

            return await Task.FromResult(item);
        }

        public async Task<PostModel> GetModel(string slug)
        {
            var model = new PostModel();

            var all = _db.Posts.AsNoTracking()
                .Where(p => p.Published > DateTime.MinValue)
                .Include(e => e.Author)
                .OrderByDescending(p => p.IsFeatured)
                .ThenByDescending(p => p.Published).ToList();

            if(all != null && all.Count > 0)
            {
                for (int i = 0; i < all.Count; i++)
                {
                    if(all[i].Slug == slug)
                    {
                        model.Post = _mapper.Map<PostItem>(all[i]);

                        if(i > 0)
                        {
                            model.Newer = _mapper.Map<PostItem>(all[i - 1]);
                        }

                        if (i + 1 < all.Count)
                        {
                            model.Older = _mapper.Map<PostItem>(all[i + 1]);
                        }

                        break;
                    }
                }
            }

            return await Task.FromResult(model);
        }

        public async Task<PostItem> SaveItem(PostItem item)
        {
            Post post;

            if(item.Id == 0)
            {
                post = _mapper.Map<Post>(item);
                _db.Posts.Add(post);
                await _db.SaveChangesAsync();

                post = _db.Posts.Single(p => p.Slug == post.Slug);
                item = _mapper.Map<PostItem>(post);
            }
            else
            {
                post = _db.Posts.Single(p => p.Id == item.Id);

                post.Title = item.Title;
                post.Slug = item.Slug;
                post.Content = item.Content;
                post.Description = item.Description ?? item.Title;
                post.Categories = item.Categories;
                post.AuthorId = item.Author.Id;
                post.Published = item.Published;
                post.IsFeatured = item.Featured;
                await _db.SaveChangesAsync();

                item.Slug = post.Slug;
            }
            return await Task.FromResult(item);
        }

        public async Task SaveCover(int postId, string asset)
        {
            var item = _db.Posts.Single(p => p.Id == postId);
            item.Cover = asset;

            await _db.SaveChangesAsync();
        } 
    }

    internal class SearchResult
    {
        public int Rank { get; set; }
        public PostItem Item { get; set; }
    }
}