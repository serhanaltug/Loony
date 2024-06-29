using Microsoft.EntityFrameworkCore.Migrations;

namespace Loony.Data.Migrations
{
    public partial class TREN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EN",
                table: "Translations",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TR",
                table: "Translations",
                nullable: false,
                defaultValue: "");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EN",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "TR",
                table: "Translations");
        }
    }
}
