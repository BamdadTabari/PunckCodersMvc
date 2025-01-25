using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataProvider.Models.Query.Blog.PostComment;
public class GetPostCommentQuery
{
    [DisplayName("PostCommentId")]
    [Required(ErrorMessage = "{0} is required")]
    public int PostCommentId { get; set; }
}
