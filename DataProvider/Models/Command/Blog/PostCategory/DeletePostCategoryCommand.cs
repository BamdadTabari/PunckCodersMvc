using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Command.Blog.PostCategory;
public class DeletePostCategoryCommand
{
    [DisplayName("PostCategoryId")]
    [Required(ErrorMessage = "{0} is required")]
    public int PostCategoryId { get; set; }

}
