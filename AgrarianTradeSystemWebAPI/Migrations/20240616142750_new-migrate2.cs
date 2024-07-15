using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgrarianTradeSystemWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class newmigrate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Orders_OrdersOrderID",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Returns_OrdersOrderID",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "OrdersOrderID",
                table: "Returns");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_OrderID",
                table: "Returns",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Orders_OrderID",
                table: "Returns",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Orders_OrderID",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Returns_OrderID",
                table: "Returns");

            migrationBuilder.AddColumn<int>(
                name: "OrdersOrderID",
                table: "Returns",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Returns_OrdersOrderID",
                table: "Returns",
                column: "OrdersOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Orders_OrdersOrderID",
                table: "Returns",
                column: "OrdersOrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }
    }
}
