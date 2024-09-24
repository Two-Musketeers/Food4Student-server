using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, string, IdentityUserClaim<string>, AppUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<RestaurantLike> RestaurantLikes { get; set; }
    public DbSet<FoodItem> FoodItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration for RestaurantLike
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

        // Configuration for Rating
        modelBuilder.Entity<Rating>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Restaurant)
            .WithMany(res => res.Ratings)
            .HasForeignKey(r => r.RestaurantId);

        // Configuration for OrderItem
        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => new { oi.OrderId, oi.FoodName });

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.OriginalFoodItem)
            .WithMany()
            .HasForeignKey(oi => oi.OriginalFoodItemId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure the relationship between AppUser and AppUserRole
        modelBuilder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        modelBuilder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

    }
}