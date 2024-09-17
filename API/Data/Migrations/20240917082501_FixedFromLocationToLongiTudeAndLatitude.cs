using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedFromLocationToLongiTudeAndLatitude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Users",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Restaurants",
                newName: "Longitude");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Restaurants",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Users",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Restaurants",
                newName: "Location");
        }
    }
}
