using DataProvider.Assistant.Pagination;
using DataProvider.Certain.Enums;

namespace DataProvider.Models.Query.Identity.User;
public class GetPagedUserQuery : DefaultPaginationFilter
{
    public bool? IsLockedOut { get; set; }
    public UserStateEnum? State { get; set; }
    public bool? IsEmailConfirmed { get; set; }
}
