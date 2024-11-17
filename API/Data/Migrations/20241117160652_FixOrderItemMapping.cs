using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderItemMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Photo_FoodItemPhotoId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Photo_FoodItemPhotoId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Photo_BannerId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Photo_LogoId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Photo_AvatarId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photo",
                table: "Photo");

            migrationBuilder.RenameTable(
                name: "Photo",
                newName: "Photos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Photos_FoodItemPhotoId",
                table: "FoodItems",
                column: "FoodItemPhotoId",
                principalTable: "Photos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Photos_FoodItemPhotoId",
                table: "OrderItems",
                column: "FoodItemPhotoId",
                principalTable: "Photos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Photos_BannerId",
                table: "Restaurants",
                column: "BannerId",
                principalTable: "Photos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Photos_LogoId",
                table: "Restaurants",
                column: "LogoId",
                principalTable: "Photos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Photos_AvatarId",
                table: "Users",
                column: "AvatarId",
                principalTable: "Photos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Photos_FoodItemPhotoId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Photos_FoodItemPhotoId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Photos_BannerId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Photos_LogoId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Photos_AvatarId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "Photo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photo",
                table: "Photo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Photo_FoodItemPhotoId",
                table: "FoodItems",
                column: "FoodItemPhotoId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Photo_FoodItemPhotoId",
                table: "OrderItems",
                column: "FoodItemPhotoId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Photo_BannerId",
                table: "Restaurants",
                column: "BannerId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Photo_LogoId",
                table: "Restaurants",
                column: "LogoId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Photo_AvatarId",
                table: "Users",
                column: "AvatarId",
                principalTable: "Photo",
                principalColumn: "Id");
        }
    }
}
