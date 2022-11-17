using Microsoft.EntityFrameworkCore.Migrations;

namespace WaterCompany.Migrations
{
    public partial class UpdateOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderDetailsTemp");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "Echelon",
                table: "Offers",
                newName: "EchelonLimit");

            migrationBuilder.AlterColumn<double>(
                name: "UnitaryValue",
                table: "OrderDetailsTemp",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<double>(
                name: "Echelon",
                table: "OrderDetailsTemp",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Echelon",
                table: "OrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UnitaryValue",
                table: "Offers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Echelon",
                table: "OrderDetailsTemp");

            migrationBuilder.DropColumn(
                name: "Echelon",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "UnitaryValue",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "EchelonLimit",
                table: "Offers",
                newName: "Echelon");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitaryValue",
                table: "OrderDetailsTemp",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderDetailsTemp",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
