using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgrarianTradeSystemWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class newmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Orders_OrdersOrderID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrdersOrderID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "OrdersOrderID",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderID",
                table: "Reviews",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Orders_OrderID",
                table: "Reviews",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Orders_OrderID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrderID",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "OrdersOrderID",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrdersOrderID",
                table: "Reviews",
                column: "OrdersOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Orders_OrdersOrderID",
                table: "Reviews",
                column: "OrdersOrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }
    }
}
