using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Order_manufacturer_id",
                table: "Order",
                column: "manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_user_id",
                table: "Order",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Manufacturer_manufacturer_id",
                table: "Order",
                column: "manufacturer_id",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_user_id",
                table: "Order",
                column: "user_id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Manufacturer_manufacturer_id",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_user_id",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_manufacturer_id",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_user_id",
                table: "Order");
        }
    }
}
