using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class VariationsConfigUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariationOptions_Variations_VariationId",
                table: "VariationOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Variations_FoodItems_FoodItemId",
                table: "Variations");

            migrationBuilder.AddForeignKey(
                name: "FK_VariationOptions_Variations_VariationId",
                table: "VariationOptions",
                column: "VariationId",
                principalTable: "Variations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Variations_FoodItems_FoodItemId",
                table: "Variations",
                column: "FoodItemId",
                principalTable: "FoodItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariationOptions_Variations_VariationId",
                table: "VariationOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Variations_FoodItems_FoodItemId",
                table: "Variations");

            migrationBuilder.AddForeignKey(
                name: "FK_VariationOptions_Variations_VariationId",
                table: "VariationOptions",
                column: "VariationId",
                principalTable: "Variations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Variations_FoodItems_FoodItemId",
                table: "Variations",
                column: "FoodItemId",
                principalTable: "FoodItems",
                principalColumn: "Id");
        }
    }
}
