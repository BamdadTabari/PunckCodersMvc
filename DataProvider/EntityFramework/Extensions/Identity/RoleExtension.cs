﻿using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Entities.Identity;
using DataProvider.Models.Query.Identity.Role;

namespace DataProvider.EntityFramework.Extensions.Identity;
public static class RoleExtension
{
    public static IQueryable<Role> ApplyFilter(this IQueryable<Role> query, GetPagedRoleQuery filter)
    {

        if (!string.IsNullOrEmpty(filter.Keyword))
            query = query.Where(x => x.Title.ToLower().Contains(filter.Keyword.ToLower().Trim()));

        if (filter.IsDeleted.HasValue)
            query = query.Where(x => x.IsDeleted == filter.IsDeleted.Value);

        return query;
    }


    public static IQueryable<Role> ApplySort(this IQueryable<Role> query, SortByEnum? sortBy)
    {
        return sortBy switch
        {
            SortByEnum.CreationDate => query.OrderBy(x => x.CreatedAt),
            SortByEnum.CreationDateDescending => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.Id)
        };
    }
}
