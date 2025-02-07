using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Entities.Blog;
using DataProvider.Models.Query.Blog.PostComment;

namespace DataProvider.EntityFramework.Extensions.Blog;
public static class PostCommentExtension
{
    public static IQueryable<PostComment> ApplyFilter(this IQueryable<PostComment> query, GetPagedPostCommentQuery filter)
    {

        if (!string.IsNullOrEmpty(filter.Keyword))
            query = query.Where(x => x.Text.ToLower().Contains(filter.Keyword.ToLower().Trim()));

        if (filter.IsDeleted.HasValue)
            query = query.Where(x => x.IsDeleted == filter.IsDeleted.Value);

        return query;
    }


    public static IQueryable<PostComment> ApplySort(this IQueryable<PostComment> query, SortByEnum? sortBy)
    {
        return sortBy switch
        {
            SortByEnum.CreationDate => query.OrderBy(x => x.CreatedAt),
            SortByEnum.CreationDateDescending => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.Id)
        };
    }
}
