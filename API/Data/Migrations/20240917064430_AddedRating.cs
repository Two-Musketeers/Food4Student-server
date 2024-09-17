using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteRestaurants_Restaurants_LikedRestaurantId",
                table: "UserFavoriteRestaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteRestaurants_Users_SourceUserId",
                table: "UserFavoriteRestaurants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavoriteRestaurants",
                table: "UserFavoriteRestaurants");

            migrationBuilder.RenameTable(
                name: "UserFavoriteRestaurants",
                newName: "RestaurantLikes");

            migrationBuilder.RenameIndex(
                name: "IX_UserFavoriteRestaurants_LikedRestaurantId",
                table: "RestaurantLikes",
                newName: "IX_RestaurantLikes_LikedRestaurantId");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Restaurants",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestaurantLikes",
                table: "RestaurantLikes",
                columns: new[] { "SourceUserId", "LikedRestaurantId" });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Stars = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RestaurantId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RestaurantId",
                table: "Ratings",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantLikes_Restaurants_LikedRestaurantId",
                table: "RestaurantLikes",
                column: "LikedRestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantLikes_Users_SourceUserId",
                table: "RestaurantLikes",
                column: "SourceUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantLikes_Restaurants_LikedRestaurantId",
                table: "RestaurantLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantLikes_Users_SourceUserId",
                table: "RestaurantLikes");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RestaurantLikes",
                table: "RestaurantLikes");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Restaurants");

            migrationBuilder.RenameTable(
                name: "RestaurantLikes",
                newName: "UserFavoriteRestaurants");

            migrationBuilder.RenameIndex(
                name: "IX_RestaurantLikes_LikedRestaurantId",
                table: "UserFavoriteRestaurants",
                newName: "IX_UserFavoriteRestaurants_LikedRestaurantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavoriteRestaurants",
                table: "UserFavoriteRestaurants",
                columns: new[] { "SourceUserId", "LikedRestaurantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteRestaurants_Restaurants_LikedRestaurantId",
                table: "UserFavoriteRestaurants",
                column: "LikedRestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteRestaurants_Users_SourceUserId",
                table: "UserFavoriteRestaurants",
                column: "SourceUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
