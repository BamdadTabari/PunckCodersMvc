using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.Models.Command.Blog.Comment;
public class EditCommentCommand
{
    [DisplayName("Comment Text")]
    [Required(ErrorMessage = "{0} is required")]
    public string Text { get; set; }
    public int PostId { get; set; }
}
