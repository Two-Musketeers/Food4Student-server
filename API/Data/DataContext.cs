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
    public DbSet<FoodItemVariation> FoodItemVariations { get; set; } = null!;
    public DbSet<Photo> Photos { get; set; } = null!;

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

        //Configuration for FoodItemVariation
        modelBuilder.Entity<FoodItemVariation>()
            .HasKey(fiv => new { fiv.FoodItemId, fiv.VariationId, fiv.VariationOptionId });

        // Configure relationships
        modelBuilder.Entity<FoodItemVariation>()
            .HasOne(fiv => fiv.FoodItem)
            .WithMany(fi => fi.FoodItemVariations)
            .HasForeignKey(fiv => fiv.FoodItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FoodItemVariation>()
            .HasOne(fiv => fiv.Variation)
            .WithMany(v => v.FoodItemVariations)
            .HasForeignKey(fiv => fiv.VariationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FoodItemVariation>()
            .HasOne(fiv => fiv.VariationOption)
            .WithMany(vo => vo.FoodItemVariations)
            .HasForeignKey(fiv => fiv.VariationOptionId)
            .OnDelete(DeleteBehavior.Cascade);

             // Configure OrderItemVariation composite key
        modelBuilder.Entity<OrderItemVariation>()
            .HasKey(oiv => new { oiv.OrderItemId, oiv.VariationId, oiv.VariationOptionId });

        // Configure relationships
        modelBuilder.Entity<OrderItemVariation>()
            .HasOne(oiv => oiv.OrderItem)
            .WithMany(oi => oi.OrderItemVariations)
            .HasForeignKey(oiv => oiv.OrderItemId);

        modelBuilder.Entity<OrderItemVariation>()
            .HasOne(oiv => oiv.Variation)
            .WithMany()
            .HasForeignKey(oiv => oiv.VariationId);

        modelBuilder.Entity<OrderItemVariation>()
            .HasOne(oiv => oiv.VariationOption)
            .WithMany()
            .HasForeignKey(oiv => oiv.VariationOptionId);
    }
}