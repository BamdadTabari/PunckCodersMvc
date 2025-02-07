using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.Models.Result.Blog.Post;
public class PostCommentResult
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string? AuthorName { get; set; } = "unknown ";
    public DateTime CreatedAt { get; set; }
    // navigation
    public int PostId { get; set; }
}
