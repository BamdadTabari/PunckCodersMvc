using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Entities.Blog;
using DataProvider.Models.Query.Blog.PostCategory;

namespace DataProvider.EntityFramework.Extensions.Blog;
public static class PostCategoryExtension
{
    public static IQueryable<PostCategory> ApplyFilter(this IQueryable<PostCategory> query, GetPagedPostCategoryQuery filter)
    {

        if (!string.IsNullOrEmpty(filter.Keyword))
            query = query.Where(x => x.Name.ToLower().Contains(filter.Keyword.ToLower().Trim()));

        if (filter.IsDeleted.HasValue)
            query = query.Where(x => x.IsDeleted == filter.IsDeleted.Value);

        return query;
    }


    public static IQueryable<PostCategory> ApplySort(this IQueryable<PostCategory> query, SortByEnum? sortBy)
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
