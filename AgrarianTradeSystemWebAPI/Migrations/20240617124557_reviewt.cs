using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgrarianTradeSystemWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class reviewt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ReturnPrice",
                table: "Returns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnQuantity",
                table: "Returns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnPrice",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "ReturnQuantity",
                table: "Returns");
        }
    }
}
