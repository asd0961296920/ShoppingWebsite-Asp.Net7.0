using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class Orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "order_number",
                keyValue: null,
                column: "order_number",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "order_number",
                table: "Order",
                type: "varchar(255)",
                nullable: false,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "order_id",
                table: "Item",
                type: "varchar(255)",
                nullable: true,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Order_order_number",
                table: "Order",
                column: "order_number");

            migrationBuilder.CreateIndex(
                name: "IX_Item_manufacturer_id",
                table: "Item",
                column: "manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Item_order_id",
                table: "Item",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_Item_user_id",
                table: "Item",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Manufacturer_manufacturer_id",
                table: "Item",
                column: "manufacturer_id",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Order_order_id",
                table: "Item",
                column: "order_id",
                principalTable: "Order",
                principalColumn: "order_number");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_User_user_id",
                table: "Item",
                column: "user_id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Manufacturer_manufacturer_id",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_Order_order_id",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_User_user_id",
                table: "Item");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Order_order_number",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Item_manufacturer_id",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_order_id",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_user_id",
                table: "Item");

            migrationBuilder.AlterColumn<string>(
                name: "order_number",
                table: "Order",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "order_id",
                table: "Item",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");

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
    }
}
