using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Command.Blog.Post;
public class CreatePostCommand
{
    [DisplayName("Title")]
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(150, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(10, ErrorMessage = "{0} must be at least {1} characters")]
    public string Title { get; set; }
    [DisplayName("ShortDescription")]
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(300, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(20, ErrorMessage = "{0} must be at least {1} characters")]
    public string ShortDescription { get; set; }
    [DisplayName("Content")]
    [Required(ErrorMessage = "{0} is required")]
    public string Content { get; set; }
    [DisplayName("IsPublished")]
    [Required(ErrorMessage = "{0} is required")]
    public bool IsPublished { get; set; }
    [DisplayName("Post Image")]
    [Required(ErrorMessage = "{0} is required")]
    public IFormFile Image { get; set; }

    // navigation
    [DisplayName("PostCategoryId")]
    [Required(ErrorMessage = "{0} is required")]
    public int PostCategoryId { get; set; }
}
