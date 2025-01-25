using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.Models.Command.Blog.Comment;
public class DeleteCommentCommand
{
    [DisplayName("Comment Id")]
    [Required(ErrorMessage = "{0} is required")]
    public int CommentId { get; set; }
}
