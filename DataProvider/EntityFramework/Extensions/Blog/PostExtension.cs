using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Entities.Blog;
using DataProvider.Models.Query.Blog.PostCategory;

namespace DataProvider.EntityFramework.Extensions.Blog;
public static class PostExtension
{
    public static IQueryable<Post> ApplyFilter(this IQueryable<Post> query, GetPagedPostQuery filter)
    {

        if (!string.IsNullOrEmpty(filter.Keyword))
            query = query.Where(x => x.Title.ToLower().Contains(filter.Keyword.ToLower().Trim())
            || x.Content.ToLower().Contains(filter.Keyword.ToLower().Trim())
            || x.ShortDescription.ToLower().Contains(filter.Keyword.ToLower().Trim()));

        // view count filter
        if (filter.MinView.HasValue)
            query = query.Where(x => x.ViewCount >= filter.MinView.Value);
        if (filter.MaxView.HasValue)
            query = query.Where(x => x.ViewCount <= filter.MaxView.Value);

        // like count filter
        if (filter.MinLike.HasValue)
            query = query.Where(x => x.LikeCount >= filter.MinLike.Value);
        if (filter.MaxLike.HasValue)
            query = query.Where(x => x.LikeCount <= filter.MaxLike.Value);


        if (filter.IsDeleted.HasValue)
            query = query.Where(x => x.IsDeleted == filter.IsDeleted.Value);

        return query;
    }


    public static IQueryable<Post> ApplySort(this IQueryable<Post> query, SortByEnum? sortBy)
    {
        return sortBy switch
        {
            SortByEnum.CreationDate => query.OrderBy(x => x.CreatedAt),
            SortByEnum.CreationDateDescending => query.OrderByDescending(x => x.CreatedAt),
            SortByEnum.UpdateDate => query.OrderBy(x => x.UpdatedAt),
            SortByEnum.UpdateDateDescending => query.OrderByDescending(x => x.UpdatedAt),
            _ => query.OrderByDescending(x => x.Id)
        };
    }
}
