using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WaterCompany.Migrations
{
    public partial class AddConsumptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConsumptionId",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Consumptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsumptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Echelon = table.Column<int>(type: "int", nullable: false),
                    UnitaryValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalConsumption = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumptions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ConsumptionId",
                table: "Invoices",
                column: "ConsumptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Consumptions_ConsumptionId",
                table: "Invoices",
                column: "ConsumptionId",
                principalTable: "Consumptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Consumptions_ConsumptionId",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "Consumptions");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_ConsumptionId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ConsumptionId",
                table: "Invoices");
        }
    }
}
