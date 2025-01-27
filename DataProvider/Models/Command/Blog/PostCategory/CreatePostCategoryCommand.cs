using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Command.Blog.PostCategory;

public class CreatePostCategoryCommand
{
    [DisplayName("Name")]
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(150, ErrorMessage = "{0} must be at most {1} characters")]
    public string Name { get; set; }

}
