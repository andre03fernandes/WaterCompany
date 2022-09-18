using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WaterCompany.Migrations
{
    public partial class ImageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Clients");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
