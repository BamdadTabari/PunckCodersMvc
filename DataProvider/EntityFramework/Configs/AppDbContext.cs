using DataProvider.EntityFramework.Entities.Identity;
using DataProvider.EntityFramework.Seeding.IdentitySeeds;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DataProvider.EntityFramework.Configs;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Apply Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.Entity<UserRole>().HasData(UserRoleSeed.All);
        modelBuilder.Entity<Role>().HasData(RoleSeed.All);
        modelBuilder.Entity<User>().HasData(UserSeed.All);

        // Creating Model
        base.OnModelCreating(modelBuilder);
    }
}
