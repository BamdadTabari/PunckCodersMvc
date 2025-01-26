using DataProvider.EntityFramework.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntityFramework.Entities.Blog;
public class Post : BaseEntity
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Content { get; set; }
    public int ViewCount { get; set; } = 0;
    public string Image { get; set; }
    public bool IsPublished { get; set; }
    public int LikeCount { get; set; } = 0;

    // navigation
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public int PostCategoryId { get; set; }
    public PostCategory PostCategory { get; set; }
    public ICollection<PostComment>? PostComments { get; set; }
}

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200).IsUnicode();
        builder.Property(x => x.ShortDescription).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.LikeCount).IsRequired();

        #region Relation
        builder.HasMany(x => x.PostComments).WithOne(x => x.Post).HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}
