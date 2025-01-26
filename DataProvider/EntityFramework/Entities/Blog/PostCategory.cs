using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntityFramework.Entities.Blog;
public class PostCategory : BaseEntity
{
    public string Name { get; set; }

    public ICollection<Post> Posts { get; set; }
}

public class PostCategoryConfiguration : IEntityTypeConfiguration<PostCategory>
{
    public void Configure(EntityTypeBuilder<PostCategory> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100).IsUnicode();

        #region Relation
        builder.HasMany(x => x.Posts).WithOne(x => x.PostCategory).HasForeignKey(x => x.PostCategoryId).OnDelete(DeleteBehavior.Restrict);
        #endregion
    }
}
