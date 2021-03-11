using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn_web.Migrations
{
    public partial class initiall2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "costOfWork",
                table: "Orders",
                type: "double(18)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "costOfTranslationServices",
                table: "Orders",
                type: "double(18)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "costOfWork",
                table: "Orders",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double(18)");

            migrationBuilder.AlterColumn<double>(
                name: "costOfTranslationServices",
                table: "Orders",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double(18)",
                oldNullable: true);
        }
    }
}
