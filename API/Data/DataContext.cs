using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<RestaurantLike> UserFavoriteRestaurants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RestaurantLike>()
            .HasKey(k => new { k.SourceUserId, k.LikedRestaurantId });

        modelBuilder.Entity<RestaurantLike>()
            .HasOne(u => u.SourceUser)
            .WithMany(r => r.FavoriteRestaurants)
            .HasForeignKey(u => u.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RestaurantLike>()
            .HasOne(r => r.LikedRestaurant)
            .WithMany(u => u.LikedByUsers)
            .HasForeignKey(r => r.LikedRestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}