using DataProvider.EntityFramework.Entities.Blog;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.EntityFramework.Entities.Identity;
public class BlacklistedToken : BaseEntity
{
    public string Token { get; set; } // The JWT token string
    public DateTime ExpiryDate { get; set; } // The expiration date of the token
    public DateTime BlacklistedOn { get; set; } = DateTime.UtcNow; // When the token was blacklisted
}

public class BlacklistedTokenConfiguration : IEntityTypeConfiguration<BlacklistedToken>
{
    public void Configure(EntityTypeBuilder<BlacklistedToken> builder)
    {
        builder.Property(x => x.Token).IsRequired();
        builder.Property(x => x.ExpiryDate).IsRequired();
        builder.Property(x => x.BlacklistedOn).IsRequired();
    }
}
