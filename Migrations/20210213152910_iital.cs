using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn_web.Migrations
{
    public partial class iital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "file",
                table: "Orders",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file",
                table: "Orders");
        }
    }
}
