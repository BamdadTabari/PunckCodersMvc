using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Command.Blog.Post;
public class DeletePostCommand
{
    [DisplayName("PostId")]
    [Required(ErrorMessage = "{0} is required")]
    public int PostId { get; set; }
}
