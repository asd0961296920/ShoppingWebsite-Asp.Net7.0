using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Product_manufacturer_id",
                table: "Product",
                column: "manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_product_class_id",
                table: "Product",
                column: "product_class_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Manufacturer_manufacturer_id",
                table: "Product",
                column: "manufacturer_id",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductClass_product_class_id",
                table: "Product",
                column: "product_class_id",
                principalTable: "ProductClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Manufacturer_manufacturer_id",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductClass_product_class_id",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_manufacturer_id",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_product_class_id",
                table: "Product");
        }
    }
}
