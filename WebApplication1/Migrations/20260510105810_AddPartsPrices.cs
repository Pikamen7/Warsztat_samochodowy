using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddPartsPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalesGrossPrice",
                table: "Parts",
                newName: "SaleGrossPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "PurchaseGrossPrice",
                table: "Parts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseGrossPrice",
                table: "Parts");

            migrationBuilder.RenameColumn(
                name: "SaleGrossPrice",
                table: "Parts",
                newName: "SalesGrossPrice");
        }
    }
}
