using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Entities.Identity;
using DataProvider.Models.Query.Identity.User;

namespace DataProvider.EntityFramework.Extensions.Identity;
public static class UserExtension
{
    public static IQueryable<User> ApplyFilter(this IQueryable<User> query, GetPagedUserQuery filter)
    {

        if (!string.IsNullOrEmpty(filter.Keyword))
            query = query.Where(x => x.Username.ToLower().Contains(filter.Keyword.ToLower().Trim()) || x.Mobile.ToLower().Contains(filter.Keyword.ToLower().Trim()));

        if (filter.IsDeleted.HasValue)
            query = query.Where(x => x.IsDeleted == filter.IsDeleted.Value);

        if (filter.IsLockedOut.HasValue)
            query = query.Where(x => x.IsLockedOut == filter.IsLockedOut.Value);

        if (filter.State.HasValue)
            query = query.Where(x => x.State == filter.State);
        return query;
    }


    public static IQueryable<User> ApplySort(this IQueryable<User> query, SortByEnum? sortBy)
    {
        return sortBy switch
        {
            SortByEnum.CreationDate => query.OrderBy(x => x.CreatedAt),
            SortByEnum.CreationDateDescending => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.Id)
        };
    }
}
