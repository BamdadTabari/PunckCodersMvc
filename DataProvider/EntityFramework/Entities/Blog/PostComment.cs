using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.EntityFramework.Entities.Blog;
public class PostComment : BaseEntity
{
    public string Text { get; set; }
    public string? AuthorName { get; set; } = " کاربر ناشناس ";

    // navigation
    public int PostId { get; set; }
    public Post Post { get; set; }

}

public class PostCommentConfiguration : IEntityTypeConfiguration<PostComment>
{
    public void Configure(EntityTypeBuilder<PostComment> builder)
    {
        builder.Property(x => x.Text).IsRequired().HasMaxLength(800);

    }
}
