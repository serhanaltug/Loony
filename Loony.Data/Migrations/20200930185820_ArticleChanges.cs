using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Loony.Data.Migrations
{
    public partial class ArticleChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "ArticleCode",
            //    table: "Articles",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT",
            //    oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Articles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Articles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Articles");

            migrationBuilder.AlterColumn<string>(
                name: "ArticleCode",
                table: "Articles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
