using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FunctionalUserFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_AppUserId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_AppUserId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Restaurants");

            migrationBuilder.CreateTable(
                name: "UserFavoriteRestaurants",
                columns: table => new
                {
                    SourceUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LikedRestaurantId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteRestaurants", x => new { x.SourceUserId, x.LikedRestaurantId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteRestaurants_Restaurants_LikedRestaurantId",
                        column: x => x.LikedRestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteRestaurants_Users_SourceUserId",
                        column: x => x.SourceUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteRestaurants_LikedRestaurantId",
                table: "UserFavoriteRestaurants",
                column: "LikedRestaurantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteRestaurants");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Restaurants",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_AppUserId",
                table: "Restaurants",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_AppUserId",
                table: "Restaurants",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
