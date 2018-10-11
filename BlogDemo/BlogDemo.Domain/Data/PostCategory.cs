using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogDemo.Domain.Data
{
    public class PostCategory
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
