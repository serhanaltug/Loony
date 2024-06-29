using Microsoft.EntityFrameworkCore.Migrations;

namespace Loony.Data.Migrations
{
    public partial class ProductCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "ProductCode",
            //    table: "Products",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductImages_Products_ProductId",
            //    table: "ProductImages",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages");

            migrationBuilder.AlterColumn<int>(
                name: "ProductCode",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
