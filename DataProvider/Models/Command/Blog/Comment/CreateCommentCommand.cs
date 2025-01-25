using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataProvider.Models.Command.Blog.Comment;
public class CreateCommentCommand
{
    [DisplayName("Comment Text")]
    [Required(ErrorMessage = "{0} is required")]
    public string Text { get; set; }
    public int PostId { get; set; }
}
