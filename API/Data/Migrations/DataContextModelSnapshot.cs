﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnedRestaurantId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnedRestaurantId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API.Entities.DeviceToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AppUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("DeviceTokens");
                });

            modelBuilder.Entity("API.Entities.FoodCategory", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RestaurantId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("FoodCategories");
                });

            modelBuilder.Entity("API.Entities.FoodItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("BasePrice")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("FoodCategoryId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FoodItemPhotoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FoodCategoryId");

                    b.HasIndex("FoodItemPhotoId");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("API.Entities.FoodItemVariation", b =>
                {
                    b.Property<string>("FoodItemId")
                        .HasColumnType("TEXT");

                    b.Property<string>("VariationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("VariationOptionId")
                        .HasColumnType("TEXT");

                    b.HasKey("FoodItemId", "VariationId", "VariationOptionId");

                    b.HasIndex("VariationId");

                    b.HasIndex("VariationOptionId");

                    b.ToTable("FoodItemVariations");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AppUserId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RatingId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RestaurantId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShippingAddressId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("RatingId");

                    b.HasIndex("RestaurantId");

                    b.HasIndex("ShippingAddressId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("API.Entities.OrderItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("FoodDescription")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FoodItemPhotoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FoodName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalFoodItemId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FoodItemPhotoId");

                    b.HasIndex("OrderId");

                    b.HasIndex("OriginalFoodItemId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("API.Entities.OrderItemVariation", b =>
                {
                    b.Property<string>("OrderItemId")
                        .HasColumnType("TEXT");

                    b.Property<string>("VariationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("VariationOptionId")
                        .HasColumnType("TEXT");

                    b.HasKey("OrderItemId", "VariationId", "VariationOptionId");

                    b.HasIndex("VariationId");

                    b.HasIndex("VariationOptionId");

                    b.ToTable("OrderItemVariation");
                });

            modelBuilder.Entity("API.Entities.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PublicId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("API.Entities.Rating", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("RestaurantId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Stars")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("API.Entities.Restaurant", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("BannerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<int?>("LogoId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BannerId");

                    b.HasIndex("LogoId");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("API.Entities.RestaurantLike", b =>
                {
                    b.Property<string>("SourceUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LikedRestaurantId")
                        .HasColumnType("TEXT");

                    b.HasKey("SourceUserId", "LikedRestaurantId");

                    b.HasIndex("LikedRestaurantId");

                    b.ToTable("RestaurantLikes");
                });

            modelBuilder.Entity("API.Entities.ShippingAddress", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AppUserId")
                        .HasColumnType("TEXT");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("ShippingAddresses");
                });

            modelBuilder.Entity("API.Entities.UserNotification", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AppUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsUnread")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("UserNotifications");
                });

            modelBuilder.Entity("API.Entities.Variation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxSelect")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinSelect")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RestaurantId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Variations");
                });

            modelBuilder.Entity("API.Entities.VariationOption", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PriceAdjustment")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VariationId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("VariationId");

                    b.ToTable("VariationOptions");
                });

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.HasOne("API.Entities.Restaurant", "OwnedRestaurant")
                        .WithMany()
                        .HasForeignKey("OwnedRestaurantId");

                    b.Navigation("OwnedRestaurant");
                });

            modelBuilder.Entity("API.Entities.DeviceToken", b =>
                {
                    b.HasOne("API.Entities.AppUser", "AppUser")
                        .WithMany("DeviceTokens")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("API.Entities.FoodCategory", b =>
                {
                    b.HasOne("API.Entities.Restaurant", "Restaurant")
                        .WithMany("FoodCategories")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("API.Entities.FoodItem", b =>
                {
                    b.HasOne("API.Entities.FoodCategory", "FoodCategory")
                        .WithMany("FoodItems")
                        .HasForeignKey("FoodCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("API.Entities.Photo", "FoodItemPhoto")
                        .WithMany()
                        .HasForeignKey("FoodItemPhotoId");

                    b.Navigation("FoodCategory");

                    b.Navigation("FoodItemPhoto");
                });

            modelBuilder.Entity("API.Entities.FoodItemVariation", b =>
                {
                    b.HasOne("API.Entities.FoodItem", "FoodItem")
                        .WithMany("FoodItemVariations")
                        .HasForeignKey("FoodItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Variation", "Variation")
                        .WithMany("FoodItemVariations")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.VariationOption", "VariationOption")
                        .WithMany("FoodItemVariations")
                        .HasForeignKey("VariationOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoodItem");

                    b.Navigation("Variation");

                    b.Navigation("VariationOption");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.HasOne("API.Entities.AppUser", "AppUser")
                        .WithMany("Orders")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("API.Entities.Rating", "Rating")
                        .WithMany()
                        .HasForeignKey("RatingId");

                    b.HasOne("API.Entities.Restaurant", "Restaurant")
                        .WithMany("Orders")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("API.Entities.ShippingAddress", "ShippingAddress")
                        .WithMany()
                        .HasForeignKey("ShippingAddressId");

                    b.Navigation("AppUser");

                    b.Navigation("Rating");

                    b.Navigation("Restaurant");

                    b.Navigation("ShippingAddress");
                });

            modelBuilder.Entity("API.Entities.OrderItem", b =>
                {
                    b.HasOne("API.Entities.Photo", "FoodItemPhoto")
                        .WithMany()
                        .HasForeignKey("FoodItemPhotoId");

                    b.HasOne("API.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");

                    b.HasOne("API.Entities.FoodItem", "OriginalFoodItem")
                        .WithMany()
                        .HasForeignKey("OriginalFoodItemId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("FoodItemPhoto");

                    b.Navigation("Order");

                    b.Navigation("OriginalFoodItem");
                });

            modelBuilder.Entity("API.Entities.OrderItemVariation", b =>
                {
                    b.HasOne("API.Entities.OrderItem", "OrderItem")
                        .WithMany("OrderItemVariations")
                        .HasForeignKey("OrderItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Variation", "Variation")
                        .WithMany()
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entities.VariationOption", "VariationOption")
                        .WithMany()
                        .HasForeignKey("VariationOptionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("OrderItem");

                    b.Navigation("Variation");

                    b.Navigation("VariationOption");
                });

            modelBuilder.Entity("API.Entities.Rating", b =>
                {
                    b.HasOne("API.Entities.Restaurant", "Restaurant")
                        .WithMany("Ratings")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("API.Entities.AppUser", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId");

                    b.Navigation("Restaurant");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.Restaurant", b =>
                {
                    b.HasOne("API.Entities.Photo", "Banner")
                        .WithMany()
                        .HasForeignKey("BannerId");

                    b.HasOne("API.Entities.Photo", "Logo")
                        .WithMany()
                        .HasForeignKey("LogoId");

                    b.Navigation("Banner");

                    b.Navigation("Logo");
                });

            modelBuilder.Entity("API.Entities.RestaurantLike", b =>
                {
                    b.HasOne("API.Entities.Restaurant", "LikedRestaurant")
                        .WithMany("LikedByUsers")
                        .HasForeignKey("LikedRestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.AppUser", "SourceUser")
                        .WithMany("FavoriteRestaurants")
                        .HasForeignKey("SourceUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LikedRestaurant");

                    b.Navigation("SourceUser");
                });

            modelBuilder.Entity("API.Entities.ShippingAddress", b =>
                {
                    b.HasOne("API.Entities.AppUser", "AppUser")
                        .WithMany("ShippingAddresses")
                        .HasForeignKey("AppUserId");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("API.Entities.UserNotification", b =>
                {
                    b.HasOne("API.Entities.AppUser", "AppUser")
                        .WithMany("UserNotifications")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("API.Entities.Variation", b =>
                {
                    b.HasOne("API.Entities.Restaurant", "Restaurant")
                        .WithMany("Variations")
                        .HasForeignKey("RestaurantId");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("API.Entities.VariationOption", b =>
                {
                    b.HasOne("API.Entities.Variation", "Variation")
                        .WithMany("VariationOptions")
                        .HasForeignKey("VariationId");

                    b.Navigation("Variation");
                });

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.Navigation("DeviceTokens");

                    b.Navigation("FavoriteRestaurants");

                    b.Navigation("Orders");

                    b.Navigation("Ratings");

                    b.Navigation("ShippingAddresses");

                    b.Navigation("UserNotifications");
                });

            modelBuilder.Entity("API.Entities.FoodCategory", b =>
                {
                    b.Navigation("FoodItems");
                });

            modelBuilder.Entity("API.Entities.FoodItem", b =>
                {
                    b.Navigation("FoodItemVariations");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("API.Entities.OrderItem", b =>
                {
                    b.Navigation("OrderItemVariations");
                });

            modelBuilder.Entity("API.Entities.Restaurant", b =>
                {
                    b.Navigation("FoodCategories");

                    b.Navigation("LikedByUsers");

                    b.Navigation("Orders");

                    b.Navigation("Ratings");

                    b.Navigation("Variations");
                });

            modelBuilder.Entity("API.Entities.Variation", b =>
                {
                    b.Navigation("FoodItemVariations");

                    b.Navigation("VariationOptions");
                });

            modelBuilder.Entity("API.Entities.VariationOption", b =>
                {
                    b.Navigation("FoodItemVariations");
                });
#pragma warning restore 612, 618
        }
    }
}
