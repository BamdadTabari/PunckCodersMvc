using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Query.Blog.PostComment;
public class GetPostCommentQuery
{
    [DisplayName("PostCommentId")]
    [Required(ErrorMessage = "{0} is required")]
    public int PostCommentId { get; set; }
}
