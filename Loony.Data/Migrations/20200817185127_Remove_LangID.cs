using Microsoft.EntityFrameworkCore.Migrations;

namespace Loony.Data.Migrations
{
    public partial class Remove_LangID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "LanguageId",
            //    table: "Translations");

            //migrationBuilder.DropColumn(
            //    name: "Text",
            //    table: "Translations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Translations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Translations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
