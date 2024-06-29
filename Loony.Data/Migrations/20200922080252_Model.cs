using Microsoft.EntityFrameworkCore.Migrations;

namespace Loony.Data.Migrations
{
    public partial class Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Models_ProductGroups_ProductGroupId",
            //    table: "Models");

            //migrationBuilder.DropIndex(
            //    name: "IX_Models_ProductGroupId",
            //    table: "Models");

            //migrationBuilder.DropColumn(
            //    name: "ProductGroupId",
            //    table: "Models");

            migrationBuilder.CreateIndex(
                name: "IX_Models_ModelGroupId",
                table: "Models",
                column: "ModelGroupId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Models_ProductGroups_ModelGroupId",
            //    table: "Models",
            //    column: "ModelGroupId",
            //    principalTable: "ProductGroups",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Models_ProductGroups_ModelGroupId",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Models_ModelGroupId",
                table: "Models");

            migrationBuilder.AddColumn<int>(
                name: "ProductGroupId",
                table: "Models",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Models_ProductGroupId",
                table: "Models",
                column: "ProductGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Models_ProductGroups_ProductGroupId",
                table: "Models",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
