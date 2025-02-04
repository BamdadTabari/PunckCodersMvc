using DataProvider.EntityFramework.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.Models.Result.Blog.Post;
public class PostResult
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Content { get; set; }
    public int ViewCount { get; set; } = 0;
    public string Image { get; set; }
    public int LikeCount { get; set; } = 0;

    // navigation
    public User Author { get; set; }
}
