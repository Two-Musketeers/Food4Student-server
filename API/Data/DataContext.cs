using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; } = null!;
    public DbSet<Rating> Ratings { get; set; } = null!;
    public DbSet<ShippingAddress> ShippingAddresses { get; set; } = null!;
    public DbSet<Restaurant> Restaurants { get; set; } = null!;
    public DbSet<RestaurantLike> RestaurantLikes { get; set; } = null!;
    public DbSet<FoodItem> FoodItems { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Variation> Variations { get; set; } = null!;
    public DbSet<VariationOption> VariationOptions { get; set; } = null!;
    public DbSet<FoodCategory> FoodCategories { get; set; } = null!;
    public DbSet<Photo> Photos { get; set; } = null!;
    public DbSet<DeviceToken> DeviceTokens { get; set; } = null!;
    public DbSet<UserNotification> UserNotifications { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Restaurant>()
                    .Property(r => r.Location)
                    .HasColumnType("geography");
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
            .OnDelete(DeleteBehavior.NoAction);

        // Configuration for Order
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Restaurant)
            .WithMany(r => r.Orders)
            .HasForeignKey(o => o.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.AppUser)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuration for OrderItem
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.OriginalFoodItem)
            .WithMany()
            .HasForeignKey(oi => oi.OriginalFoodItemId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configuration for Restaurant -> FoodCategories
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.FoodCategories)
            .WithOne(fc => fc.Restaurant)
            .HasForeignKey(fc => fc.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure FoodCategory -> FoodItems relationship
        modelBuilder.Entity<FoodCategory>()
            .HasMany(fc => fc.FoodItems)
            .WithOne(fi => fi.FoodCategory)
            .HasForeignKey(fi => fi.FoodCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Restaurant -> Ratings relationship
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Ratings)
            .WithOne(rt => rt.Restaurant)
            .HasForeignKey(rt => rt.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Variation>()
            .HasOne(v => v.FoodItem)
            .WithMany(fi => fi.Variations)
            .HasForeignKey(v => v.FoodItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VariationOption>()
            .HasOne(vo => vo.Variation)
            .WithMany(v => v.VariationOptions)
            .HasForeignKey(vo => vo.VariationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FoodCategory>()
                .HasMany(fc => fc.FoodItems)
                .WithOne(fi => fi.FoodCategory)
                .HasForeignKey(fi => fi.FoodCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}