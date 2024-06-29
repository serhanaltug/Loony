using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Loony.Data.Migrations
{
    public partial class Files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductChains_Accessories_AccessoryId",
            //    table: "ProductChains");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductChains_Articles_ArticleId",
            //    table: "ProductChains");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductChains_Models_ModelId",
            //    table: "ProductChains");

            //migrationBuilder.AlterColumn<int>(
            //    name: "ModelId",
            //    table: "ProductChains",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER");

            //migrationBuilder.AlterColumn<int>(
            //    name: "ArticleId",
            //    table: "ProductChains",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER");

            //migrationBuilder.AlterColumn<int>(
            //    name: "AccessoryId",
            //    table: "ProductChains",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "FileGroups",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileGroupName = table.Column<string>(maxLength: 50, nullable: false),
                    RelatedGrupID = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileGroups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FileGroups_FileGroups_RelatedGrupID",
                        column: x => x.RelatedGrupID,
                        principalTable: "FileGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserID = table.Column<Guid>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: true),
                    FileName = table.Column<string>(maxLength: 255, nullable: false),
                    FileType = table.Column<int>(nullable: true),
                    MIMEType = table.Column<string>(maxLength: 100, nullable: true),
                    FileContent = table.Column<byte[]>(nullable: false),
                    CreationDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    FileGroupId = table.Column<int>(nullable: true),
                    BeginDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "smalldatetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_FileGroups_FileGroupId",
                        column: x => x.FileGroupId,
                        principalTable: "FileGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Files_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Files_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "File_Tag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelId = table.Column<int>(nullable: false),
                    FileId = table.Column<Guid>(nullable: true),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_File_Tag_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_File_Tag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_Tag_FileId",
                table: "File_Tag",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_File_Tag_TagId",
                table: "File_Tag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_FileGroups_RelatedGrupID",
                table: "FileGroups",
                column: "RelatedGrupID");

            migrationBuilder.CreateIndex(
                name: "IX_Files_FileGroupId",
                table: "Files",
                column: "FileGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ProductId",
                table: "Files",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UserID",
                table: "Files",
                column: "UserID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductChains_Accessories_AccessoryId",
            //    table: "ProductChains",
            //    column: "AccessoryId",
            //    principalTable: "Accessories",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductChains_Articles_ArticleId",
            //    table: "ProductChains",
            //    column: "ArticleId",
            //    principalTable: "Articles",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductChains_Models_ModelId",
            //    table: "ProductChains",
            //    column: "ModelId",
            //    principalTable: "Models",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductChains_Accessories_AccessoryId",
                table: "ProductChains");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductChains_Articles_ArticleId",
                table: "ProductChains");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductChains_Models_ModelId",
                table: "ProductChains");

            migrationBuilder.DropTable(
                name: "File_Tag");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "FileGroups");

            migrationBuilder.AlterColumn<int>(
                name: "ModelId",
                table: "ProductChains",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ArticleId",
                table: "ProductChains",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccessoryId",
                table: "ProductChains",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductChains_Accessories_AccessoryId",
                table: "ProductChains",
                column: "AccessoryId",
                principalTable: "Accessories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductChains_Articles_ArticleId",
                table: "ProductChains",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductChains_Models_ModelId",
                table: "ProductChains",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
