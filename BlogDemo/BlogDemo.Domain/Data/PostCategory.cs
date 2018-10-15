using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogDemo.Domain.Data
{
    public class PostCategory
    {
        [Key]
        public int Id { get; set; }       
        public string Name { get; set; }
        public virtual ICollection<Post> Posts { get; set; }      
    }
}
