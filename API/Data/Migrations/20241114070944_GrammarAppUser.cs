using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class GrammarAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Restaurants_OwnedRestaurantsId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "OwnedRestaurantsId",
                table: "Users",
                newName: "OwnedRestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_OwnedRestaurantsId",
                table: "Users",
                newName: "IX_Users_OwnedRestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Restaurants_OwnedRestaurantId",
                table: "Users",
                column: "OwnedRestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Restaurants_OwnedRestaurantId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "OwnedRestaurantId",
                table: "Users",
                newName: "OwnedRestaurantsId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_OwnedRestaurantId",
                table: "Users",
                newName: "IX_Users_OwnedRestaurantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Restaurants_OwnedRestaurantsId",
                table: "Users",
                column: "OwnedRestaurantsId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
