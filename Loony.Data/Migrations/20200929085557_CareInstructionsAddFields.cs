using Microsoft.EntityFrameworkCore.Migrations;

namespace Loony.Data.Migrations
{
    public partial class CareInstructionsAddFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CareInstructions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "CareInstructions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CareInstructions");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "CareInstructions");
        }
    }
}
