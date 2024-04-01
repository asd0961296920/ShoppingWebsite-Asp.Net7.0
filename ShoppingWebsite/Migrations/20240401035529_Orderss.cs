using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class Orderss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Item_product_id",
                table: "Item",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Product_product_id",
                table: "Item",
                column: "product_id",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Product_product_id",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_product_id",
                table: "Item");
        }
    }
}
