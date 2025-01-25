using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Command.Blog.Comment;
public class DeleteCommentCommand
{
    [DisplayName("Comment Id")]
    [Required(ErrorMessage = "{0} is required")]
    public int CommentId { get; set; }
}
