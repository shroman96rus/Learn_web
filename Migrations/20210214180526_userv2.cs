using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn_web.Migrations
{
    public partial class userv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "userPassword");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "userPassword",
                table: "Users",
                newName: "password");
        }
    }
}
