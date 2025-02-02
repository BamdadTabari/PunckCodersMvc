using DataProvider.Assistant.Extension;
using DataProvider.Certain.Constants;
using DataProvider.Certain.Enums;
using DataProvider.EntityFramework.Entities.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataProvider.EntityFramework.Entities.Identity;

public class User : BaseEntity
{
    #region Identity

    public string Username { get; set; }

    public string Mobile { get; set; }

    public string Email { get; set; }
    public bool IsEmailConfirmed { get; set; }

    #endregion

    #region Login

    public string PasswordHash { get; set; }
    public DateTime? LastPasswordChangeTime { get; set; }

    public int FailedLoginCount { get; set; }
    public DateTime? LockoutEndTime { get; set; }

    public DateTime? LastLoginDate { get; set; }

    #endregion

    #region Profile
    public UserStateEnum State { get; set; }

    #endregion

    #region Management

    public string SecurityStamp { get; set; }
    public string ConcurrencyStamp { get; set; }
    public bool IsLockedOut { get; set; }
    #endregion

    #region Navigations
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<Post> Posts { get; set; }
    public ICollection<PostLike> PostLikes { get; set; }
    #endregion
}

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(b => b.Username).IsUnique();

        #region Mappings

        builder.Property(b => b.Username)
            .HasMaxLength(Defaults.UsernameMaxLength)
            .IsRequired();

        builder.Property(b => b.Mobile)
            .IsRequired();

        builder.Property(b => b.Email)
            .IsRequired();

        builder.Property(b => b.PasswordHash)
            .HasMaxLength(Defaults.PasswordHashLength)
            .IsRequired();

        builder.Property(b => b.SecurityStamp)
            .HasMaxLength(Defaults.SecurityStampLength)
            .IsFixedLength();

        builder.Property(b => b.ConcurrencyStamp)
            .IsConcurrencyToken()
            .HasMaxLength(Defaults.SecurityStampLength);

        #endregion

        #region Conversions

        builder.Property(x => x.State)
            .HasConversion(new EnumToStringConverter<UserStateEnum>());

        #endregion

        #region Navigations

        builder
            .HasMany(x => x.UserRoles)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.UserRoles)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}