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
    [Migration("20240917020119_DeezNut")]
    partial class DeezNut
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("AvatarId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AvatarId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API.Entities.FoodItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FoodItemPhotoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FoodItemPhotoId");

                    b.HasIndex("RestaurantId");

                    b.ToTable("FoodItems");
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

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("API.Entities.Restaurant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BannerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LogoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("BannerId");

                    b.HasIndex("LogoId");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.HasOne("API.Entities.Photo", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Avatar");
                });

            modelBuilder.Entity("API.Entities.FoodItem", b =>
                {
                    b.HasOne("API.Entities.Photo", "FoodItemPhoto")
                        .WithMany()
                        .HasForeignKey("FoodItemPhotoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Restaurant", "Restaurant")
                        .WithMany("Menu")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoodItemPhoto");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("API.Entities.Restaurant", b =>
                {
                    b.HasOne("API.Entities.AppUser", null)
                        .WithMany("FavoriteRestaurants")
                        .HasForeignKey("AppUserId");

                    b.HasOne("API.Entities.Photo", "Banner")
                        .WithMany()
                        .HasForeignKey("BannerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Photo", "Logo")
                        .WithMany()
                        .HasForeignKey("LogoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Banner");

                    b.Navigation("Logo");
                });

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.Navigation("FavoriteRestaurants");
                });

            modelBuilder.Entity("API.Entities.Restaurant", b =>
                {
                    b.Navigation("Menu");
                });
#pragma warning restore 612, 618
        }
    }
}
