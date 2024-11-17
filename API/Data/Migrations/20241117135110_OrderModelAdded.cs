using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderModelAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "OrderItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FoodDescription",
                table: "OrderItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoodItemPhotoId",
                table: "OrderItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "OrderItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_FoodItemPhotoId",
                table: "OrderItems",
                column: "FoodItemPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Photo_FoodItemPhotoId",
                table: "OrderItems",
                column: "FoodItemPhotoId",
                principalTable: "Photo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Photo_FoodItemPhotoId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_FoodItemPhotoId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FoodDescription",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "FoodItemPhotoId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "OrderItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
