using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixThisHellHole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Photos_FoodItemPhotoId",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "OrderItemVariation");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_FoodItemPhotoId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "FoodItemPhotoId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FoodItemPhotoUrl",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Variations",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodItemPhotoUrl",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Variations",
                table: "OrderItems");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoodItemPhotoId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderItemVariation",
                columns: table => new
                {
                    OrderItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VariationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VariationOptionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemVariation", x => new { x.OrderItemId, x.VariationId, x.VariationOptionId });
                    table.ForeignKey(
                        name: "FK_OrderItemVariation_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemVariation_VariationOptions_VariationOptionId",
                        column: x => x.VariationOptionId,
                        principalTable: "VariationOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemVariation_Variations_VariationId",
                        column: x => x.VariationId,
                        principalTable: "Variations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_FoodItemPhotoId",
                table: "OrderItems",
                column: "FoodItemPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemVariation_VariationId",
                table: "OrderItemVariation",
                column: "VariationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemVariation_VariationOptionId",
                table: "OrderItemVariation",
                column: "VariationOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Photos_FoodItemPhotoId",
                table: "OrderItems",
                column: "FoodItemPhotoId",
                principalTable: "Photos",
                principalColumn: "Id");
        }
    }
}
