using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Query.Blog.PostCategory;
public class GetPostQuery
{
    [DisplayName("PostId")]
    [Required(ErrorMessage = "{0} is required")]
    public int PostId { get; set; }
}
