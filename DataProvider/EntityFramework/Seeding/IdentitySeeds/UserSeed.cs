using DataProvider.Assistant.Helpers;
using DataProvider.Certain.Constants;
using DataProvider.Certain.Enums;
using DataProvider.EntityFramework.Entities.Identity;

namespace DataProvider.EntityFramework.Seeding.IdentitySeeds;

public static class UserSeed
{
    public static List<User> All => new List<User>()
    {
        new User()
        {
            Id = 1,
            Email = "bamdadtabari@outlook.com",
            IsEmailConfirmed = true,
            FailedLoginCount = 0,
            IsLockedOut = false,
            Mobile = "09301724389",
            State = UserStateEnum.Active,
            Username = "ill",
            PasswordHash = PasswordHasher.Hash("QAZqaz!@#123"),
            ConcurrencyStamp = StampGenerator.CreateSecurityStamp(Defaults.SecurityStampLength),
            SecurityStamp = StampGenerator.CreateSecurityStamp(Defaults.SecurityStampLength),
            LastPasswordChangeTime =  new DateTime(2025, 1, 1, 12, 0, 0),
            CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0),
            UpdatedAt = new DateTime(2025, 1, 1, 12, 0, 0),
            IsDeleted = false,
        }
    };
}