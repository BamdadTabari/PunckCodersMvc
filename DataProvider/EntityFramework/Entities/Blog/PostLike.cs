using DataProvider.EntityFramework.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataProvider.EntityFramework.Entities.Blog;
public class PostLike: BaseEntity
{
    public int PostId { get; set; }
    public int UserId { get; set; }

    public Post Post { get; set; }
    public User User { get; set; }
}
public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
{
    public void Configure(EntityTypeBuilder<PostLike> builder)
    {
        builder.HasKey(x => x.Id);

        #region Mappings

        builder.Property(b => b.PostId)
            .IsRequired();

        builder.Property(b => b.UserId)
            .IsRequired();
        #endregion

        #region Navigations

        builder
            .HasOne(x => x.Post)
            .WithMany(x => x.PostLikes)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.PostLikes)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}