using DataProvider.EntityFramework.Entities.Identity;

namespace DataProvider.EntityFramework.Seeding.IdentitySeeds;

public static class RoleSeed
{
    public static List<Role> All => new List<Role>
    {
        new Role()
        {
            Id = 1,
            Title = "Owner",
            CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0),
            UpdatedAt = DateTime.Now,
            IsDeleted = false,
        },
         new Role()
        {
            Id = 2,
            Title = "Admin",
            CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0),
            UpdatedAt = new DateTime(2025, 1, 1, 12, 0, 0),
            IsDeleted = false,
        },
         new Role()
        {
            Id = 3,
            Title = "Writer",
            CreatedAt =  new DateTime(2025, 1, 1, 12, 0, 0),
            UpdatedAt =  new DateTime(2025, 1, 1, 12, 0, 0),
            IsDeleted = false,
        },
         new Role()
        {
            Id = 4,
            Title = "Reader",
            CreatedAt =  new DateTime(2025, 1, 1, 12, 0, 0),
            UpdatedAt =  new DateTime(2025, 1, 1, 12, 0, 0),
            IsDeleted = false,
        }
    };
}