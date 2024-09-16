using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class StillUpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Restaurant_RestaurantId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Photo_BannerId",
                table: "Restaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Photo_LogoId",
                table: "Restaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Users_AppUserId",
                table: "Restaurant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurant",
                table: "Restaurant");

            migrationBuilder.RenameTable(
                name: "Restaurant",
                newName: "Restaurants");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurant_LogoId",
                table: "Restaurants",
                newName: "IX_Restaurants_LogoId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurant_BannerId",
                table: "Restaurants",
                newName: "IX_Restaurants_BannerId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurant_AppUserId",
                table: "Restaurants",
                newName: "IX_Restaurants_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Restaurants_RestaurantId",
                table: "FoodItems",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Photo_BannerId",
                table: "Restaurants",
                column: "BannerId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Photo_LogoId",
                table: "Restaurants",
                column: "LogoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_AppUserId",
                table: "Restaurants",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Restaurants_RestaurantId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Photo_BannerId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Photo_LogoId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_AppUserId",
                table: "Restaurants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants");

            migrationBuilder.RenameTable(
                name: "Restaurants",
                newName: "Restaurant");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_LogoId",
                table: "Restaurant",
                newName: "IX_Restaurant_LogoId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_BannerId",
                table: "Restaurant",
                newName: "IX_Restaurant_BannerId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_AppUserId",
                table: "Restaurant",
                newName: "IX_Restaurant_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurant",
                table: "Restaurant",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Restaurant_RestaurantId",
                table: "FoodItems",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Photo_BannerId",
                table: "Restaurant",
                column: "BannerId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Photo_LogoId",
                table: "Restaurant",
                column: "LogoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Users_AppUserId",
                table: "Restaurant",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
