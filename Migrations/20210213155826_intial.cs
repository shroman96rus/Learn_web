using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn_web.Migrations
{
    public partial class intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "path",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "path",
                table: "Orders");

            migrationBuilder.AddColumn<byte[]>(
                name: "file",
                table: "Orders",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
