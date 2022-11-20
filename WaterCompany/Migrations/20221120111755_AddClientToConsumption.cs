using Microsoft.EntityFrameworkCore.Migrations;

namespace WaterCompany.Migrations
{
    public partial class AddClientToConsumption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Consumptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consumptions_ClientId",
                table: "Consumptions",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumptions_Clients_ClientId",
                table: "Consumptions",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consumptions_Clients_ClientId",
                table: "Consumptions");

            migrationBuilder.DropIndex(
                name: "IX_Consumptions_ClientId",
                table: "Consumptions");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Consumptions");
        }
    }
}
