﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241123015615_RatingModelChange")]
    partial class RatingModelChange
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("API.Entities.FoodItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FoodItemPhotoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RestaurantId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FoodItemPhotoId");

                    b.HasIndex("RestaurantId");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AppUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RatingId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RestaurantId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ShippingAddressId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("RatingId");

                    b.HasIndex("RestaurantId");

                    b.HasIndex("ShippingAddressId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("API.Entities.OrderItem", b =>
                {
                    b.Property<string>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FoodName")
                        .HasColumnType("TEXT");

                    b.Property<string>("FoodDescription")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FoodItemPhotoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalFoodItemId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("OrderId", "FoodName");

                    b.HasIndex("FoodItemPhotoId");

                    b.HasIndex("OriginalFoodItemId");

                    b.ToTable("OrderItems");
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
                        .IsRequired()
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

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.HasOne("API.Entities.Restaurant", "OwnedRestaurant")
                        .WithMany()
                        .HasForeignKey("OwnedRestaurantId");

                    b.Navigation("OwnedRestaurant");
                });

            modelBuilder.Entity("API.Entities.FoodItem", b =>
                {
                    b.HasOne("API.Entities.Photo", "FoodItemPhoto")
                        .WithMany()
                        .HasForeignKey("FoodItemPhotoId");

                    b.HasOne("API.Entities.Restaurant", "Restaurant")
                        .WithMany("Menu")
                        .HasForeignKey("RestaurantId");

                    b.Navigation("FoodItemPhoto");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.HasOne("API.Entities.AppUser", "AppUser")
                        .WithMany("Orders")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entities.Rating", "Rating")
                        .WithMany()
                        .HasForeignKey("RatingId");

                    b.HasOne("API.Entities.Restaurant", "Restaurant")
                        .WithMany("Orders")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

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
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.FoodItem", "OriginalFoodItem")
                        .WithMany()
                        .HasForeignKey("OriginalFoodItemId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("FoodItemPhoto");

                    b.Navigation("Order");

                    b.Navigation("OriginalFoodItem");
                });

            modelBuilder.Entity("API.Entities.Rating", b =>
                {
                    b.HasOne("API.Entities.Restaurant", "Restaurant")
                        .WithMany("Ratings")
                        .HasForeignKey("RestaurantId");

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
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.Navigation("FavoriteRestaurants");

                    b.Navigation("Orders");

                    b.Navigation("Ratings");

                    b.Navigation("ShippingAddresses");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("API.Entities.Restaurant", b =>
                {
                    b.Navigation("LikedByUsers");

                    b.Navigation("Menu");

                    b.Navigation("Orders");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
