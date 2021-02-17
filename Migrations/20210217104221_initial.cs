using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn_web.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateOrder = table.Column<DateTime>(type: "datetime2", nullable: false),
                    clientData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    originalLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    translateLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    costOfWork = table.Column<double>(type: "float", nullable: false),
                    costOfTranslationServices = table.Column<double>(type: "float", nullable: true),
                    Translator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
