using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Loony.Data.Migrations
{
    public partial class FileRelationUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Files_Products_ProductId",
            //    table: "Files");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Files_Users_UserID",
            //    table: "Files");

            //migrationBuilder.DropIndex(
            //    name: "IX_Files_ProductId",
            //    table: "Files");

            //migrationBuilder.DropIndex(
            //    name: "IX_Files_UserID",
            //    table: "Files");

            //migrationBuilder.DropColumn(
            //    name: "ProductId",
            //    table: "Files");

            //migrationBuilder.DropColumn(
            //    name: "UserID",
            //    table: "Files");

            migrationBuilder.AddColumn<Guid>(
                name: "RelatedGuid",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelatedId",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedTable",
                table: "Files",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatedGuid",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "RelatedId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "RelatedTable",
                table: "Files");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Files",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "Files",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_ProductId",
                table: "Files",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UserID",
                table: "Files",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Products_ProductId",
                table: "Files",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_UserID",
                table: "Files",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
