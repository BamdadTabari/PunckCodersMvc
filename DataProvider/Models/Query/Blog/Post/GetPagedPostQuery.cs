using DataProvider.Assistant.Pagination;
using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Query.Blog.PostCategory;
public class GetPagedPostQuery : DefaultPaginationFilter
{
    public int? MinView { get; set; }
    [Compare("MinView", ErrorMessage = "MinView must be less than MaxView")]
    public int? MaxView { get; set; }

    public int? MinLike { get; set; }
    [Compare("MinLike", ErrorMessage = "MinLike must be less than MaxLike")]
    public int? MaxLike { get; set; }

}
