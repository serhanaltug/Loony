using Microsoft.EntityFrameworkCore.Migrations;

namespace Loony.Data.Migrations
{
    public partial class MenuUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Admin",
            //    table: "Menu");

            migrationBuilder.AddColumn<bool>(
                name: "IsSuperUser",
                table: "Users",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "AdminMenu",
            //    table: "Menu",
            //    nullable: false,
            //    defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Everyone",
                table: "Menu",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SuperUser",
                table: "Menu",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuperUser",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AdminMenu",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "Everyone",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "SuperUser",
                table: "Menu");

            migrationBuilder.AddColumn<bool>(
                name: "Admin",
                table: "Menu",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
