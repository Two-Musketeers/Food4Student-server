using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixVariationsMaMinRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsMultiSelect",
                table: "Variations",
                newName: "MinSelect");

            migrationBuilder.AddColumn<int>(
                name: "MaxSelect",
                table: "Variations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxSelect",
                table: "Variations");

            migrationBuilder.RenameColumn(
                name: "MinSelect",
                table: "Variations",
                newName: "IsMultiSelect");
        }
    }
}
