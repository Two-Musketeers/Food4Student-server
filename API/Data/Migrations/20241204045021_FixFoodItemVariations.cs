using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixFoodItemVariations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Restaurants_RestaurantId",
                table: "FoodItems");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_RestaurantId",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "FoodItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RestaurantId",
                table: "FoodItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_RestaurantId",
                table: "FoodItems",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Restaurants_RestaurantId",
                table: "FoodItems",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
