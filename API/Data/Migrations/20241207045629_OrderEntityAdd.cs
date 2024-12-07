using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderEntityAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemVariation_VariationOptions_VariationOptionId",
                table: "OrderItemVariation");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemVariation_Variations_VariationId",
                table: "OrderItemVariation");

            migrationBuilder.AddColumn<int>(
                name: "TotalPrice",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemVariation_VariationOptions_VariationOptionId",
                table: "OrderItemVariation",
                column: "VariationOptionId",
                principalTable: "VariationOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemVariation_Variations_VariationId",
                table: "OrderItemVariation",
                column: "VariationId",
                principalTable: "Variations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemVariation_VariationOptions_VariationOptionId",
                table: "OrderItemVariation");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemVariation_Variations_VariationId",
                table: "OrderItemVariation");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemVariation_VariationOptions_VariationOptionId",
                table: "OrderItemVariation",
                column: "VariationOptionId",
                principalTable: "VariationOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemVariation_Variations_VariationId",
                table: "OrderItemVariation",
                column: "VariationId",
                principalTable: "Variations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
