using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Query.Blog.PostCategory;
public class GetPostCategoryQuery
{
    [DisplayName("PostCategoryId")]
    [Required(ErrorMessage = "{0} is required")]
    public int PostCategoryId { get; set; }
}
