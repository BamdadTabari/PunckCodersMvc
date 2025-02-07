using DataProvider.EntityFramework.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.Models.Result.Blog.Post;
public class PostResult
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Content { get; set; }
    public int ViewCount { get; set; } = 0;
    public string Image { get; set; }
    public int LikeCount { get; set; } = 0;
    public bool? IsUserLiked { get; set; }
    // navigation
    public string Author { get; set; }
    public int PostCategoryId { get; set; }
    public string PostCategoryName { get; set; }

    public DateTime CreatedAt { get; set; }
    public ICollection<PostCommentResult> PostCommentResults { get; set; }
}
