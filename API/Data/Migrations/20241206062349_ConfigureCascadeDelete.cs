using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodCategories_Restaurants_RestaurantId",
                table: "FoodCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_FoodCategories_FoodCategoryId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Restaurants_RestaurantId",
                table: "Ratings");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodCategories_Restaurants_RestaurantId",
                table: "FoodCategories",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_FoodCategories_FoodCategoryId",
                table: "FoodItems",
                column: "FoodCategoryId",
                principalTable: "FoodCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Restaurants_RestaurantId",
                table: "Ratings",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodCategories_Restaurants_RestaurantId",
                table: "FoodCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_FoodCategories_FoodCategoryId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Restaurants_RestaurantId",
                table: "Ratings");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodCategories_Restaurants_RestaurantId",
                table: "FoodCategories",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_FoodCategories_FoodCategoryId",
                table: "FoodItems",
                column: "FoodCategoryId",
                principalTable: "FoodCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Restaurants_RestaurantId",
                table: "Ratings",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
