using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RoleRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_AppUserId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Photo_AvatarId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_AppUserId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Restaurants");

            migrationBuilder.AlterColumn<int>(
                name: "AvatarId",
                table: "Users",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "AppRoleId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnedRestaurantsId",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShippingAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingAddress_Users_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AppRoleId",
                table: "Users",
                column: "AppRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OwnedRestaurantsId",
                table: "Users",
                column: "OwnedRestaurantsId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_AppUserId",
                table: "ShippingAddress",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Photo_AvatarId",
                table: "Users",
                column: "AvatarId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Restaurants_OwnedRestaurantsId",
                table: "Users",
                column: "OwnedRestaurantsId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_AppRoleId",
                table: "Users",
                column: "AppRoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Photo_AvatarId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Restaurants_OwnedRestaurantsId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_AppRoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ShippingAddress");

            migrationBuilder.DropIndex(
                name: "IX_Users_AppRoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OwnedRestaurantsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AppRoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OwnedRestaurantsId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "AvatarId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Users",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Users",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Restaurants",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_AppUserId",
                table: "Restaurants",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_AppUserId",
                table: "Restaurants",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Photo_AvatarId",
                table: "Users",
                column: "AvatarId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
