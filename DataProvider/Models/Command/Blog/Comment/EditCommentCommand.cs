using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Command.Blog.Comment;
public class EditCommentCommand
{
    [DisplayName("Comment Text")]
    [Required(ErrorMessage = "{0} is required")]
    public string Text { get; set; }
    [DisplayName("Comment Id")]
    [Required(ErrorMessage = "{0} is required")]
    public int CommentId { get; set; }
}
