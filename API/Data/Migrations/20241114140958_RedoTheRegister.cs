using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RedoTheRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingAddress_Users_AppUserId",
                table: "ShippingAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingAddress",
                table: "ShippingAddress");

            migrationBuilder.RenameTable(
                name: "ShippingAddress",
                newName: "ShippingAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingAddress_AppUserId",
                table: "ShippingAddresses",
                newName: "IX_ShippingAddresses_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingAddresses",
                table: "ShippingAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingAddresses_Users_AppUserId",
                table: "ShippingAddresses",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingAddresses_Users_AppUserId",
                table: "ShippingAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingAddresses",
                table: "ShippingAddresses");

            migrationBuilder.RenameTable(
                name: "ShippingAddresses",
                newName: "ShippingAddress");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingAddresses_AppUserId",
                table: "ShippingAddress",
                newName: "IX_ShippingAddress_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingAddress",
                table: "ShippingAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingAddress_Users_AppUserId",
                table: "ShippingAddress",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
