using Microsoft.EntityFrameworkCore.Migrations;

namespace WaterCompany.Migrations
{
    public partial class UpdateConsumption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consumptions_AspNetUsers_UserId",
                table: "Consumptions");

            migrationBuilder.DropIndex(
                name: "IX_Consumptions_UserId",
                table: "Consumptions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Consumptions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Consumptions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consumptions_UserId",
                table: "Consumptions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumptions_AspNetUsers_UserId",
                table: "Consumptions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
