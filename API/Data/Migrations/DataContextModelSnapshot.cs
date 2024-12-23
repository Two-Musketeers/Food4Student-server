﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OwnedRestaurantId")
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OwnedRestaurantId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API.Entities.DeviceToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AppUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("DeviceTokens");
                });

            modelBuilder.Entity("API.Entities.FoodCategory", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RestaurantId")
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("FoodCategories");
                });

            modelBuilder.Entity("API.Entities.FoodItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("BasePrice")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FoodCategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("FoodItemPhotoId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FoodCategoryId");

                    b.HasIndex("FoodItemPhotoId");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AppUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RatingId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RestaurantId")
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("RatingId");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("API.Entities.OrderItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FoodDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FoodItemPhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FoodName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OriginalFoodItemId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Variations")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("OriginalFoodItemId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("API.Entities.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PublicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("API.Entities.Rating", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("RestaurantId")
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("Stars")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("API.Entities.Restaurant", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BannerId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<Point>("Location")
                        .IsRequired()
                        .HasColumnType("geography");

                    b.Property<int?>("LogoId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BannerId");

                    b.HasIndex("LogoId");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("API.Entities.RestaurantLike", b =>
                {
                    b.Property<string>("SourceUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LikedRestaurantId")
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("SourceUserId", "LikedRestaurantId");

                    b.HasIndex("LikedRestaurantId");

                    b.ToTable("RestaurantLikes");
                });

            modelBuilder.Entity("API.Entities.ShippingAddress", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AppUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BuildingNote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LocationType")
                        .HasColumnType("int");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OtherLocationTypeTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("ShippingAddresses");
                });

            modelBuilder.Entity("API.Entities.UserNotification", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AppUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsUnread")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("UserNotifications");
                });

            modelBuilder.Entity("API.Entities.Variation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FoodItemId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("MaxSelect")
                        .HasColumnType("int");

                    b.Property<int>("MinSelect")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RestaurantId")
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("FoodItemId");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Variations");
                });

            modelBuilder.Entity("API.Entities.VariationOption", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PriceAdjustment")
                        .HasColumnType("int");

                    b.Property<string>("VariationId")
                        .HasColumnType("nvarchar(450)");

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

                    b.Navigation("AppUser");

                    b.Navigation("Rating");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("API.Entities.OrderItem", b =>
                {
                    b.HasOne("API.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");

                    b.HasOne("API.Entities.FoodItem", "OriginalFoodItem")
                        .WithMany()
                        .HasForeignKey("OriginalFoodItemId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Order");

                    b.Navigation("OriginalFoodItem");
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
                        .OnDelete(DeleteBehavior.NoAction)
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
                        .HasForeignKey("AppUserId");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("API.Entities.Variation", b =>
                {
                    b.HasOne("API.Entities.FoodItem", "FoodItem")
                        .WithMany("Variations")
                        .HasForeignKey("FoodItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("API.Entities.Restaurant", null)
                        .WithMany("Variations")
                        .HasForeignKey("RestaurantId");

                    b.Navigation("FoodItem");
                });

            modelBuilder.Entity("API.Entities.VariationOption", b =>
                {
                    b.HasOne("API.Entities.Variation", "Variation")
                        .WithMany("VariationOptions")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade);

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
                    b.Navigation("Variations");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
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
                    b.Navigation("VariationOptions");
                });
#pragma warning restore 612, 618
        }
    }
}
