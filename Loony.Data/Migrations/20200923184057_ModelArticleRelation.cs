using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Loony.Data.Migrations
{
    public partial class ModelArticleRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Product_Model");

            //migrationBuilder.DropColumn(
            //    name: "CareInstructions",
            //    table: "Products");

            //migrationBuilder.DropColumn(
            //    name: "Composition",
            //    table: "Products");

            //migrationBuilder.DropColumn(
            //    name: "Line",
            //    table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Season",
                table: "Models",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Models",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArticleCode = table.Column<string>(nullable: true),
                    ArticleName = table.Column<string>(nullable: true),
                    DesignCode = table.Column<string>(nullable: true),
                    ColorCode = table.Column<string>(nullable: true),
                    Composition = table.Column<string>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Supplier = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DesignTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DesignTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product_Model_Article",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<Guid>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    ArticleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Model_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Model_Article_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Model_Article_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Model_Article_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Article_DesignType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArticleId = table.Column<int>(nullable: false),
                    DesignTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article_DesignType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Article_DesignType_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Article_DesignType_DesignTypes_DesignTypeId",
                        column: x => x.DesignTypeId,
                        principalTable: "DesignTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Article_DesignType_ArticleId",
                table: "Article_DesignType",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_DesignType_DesignTypeId",
                table: "Article_DesignType",
                column: "DesignTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Model_Article_ArticleId",
                table: "Product_Model_Article",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Model_Article_ModelId",
                table: "Product_Model_Article",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Model_Article_ProductId",
                table: "Product_Model_Article",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Article_DesignType");

            migrationBuilder.DropTable(
                name: "Product_Model_Article");

            migrationBuilder.DropTable(
                name: "DesignTypes");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropColumn(
                name: "Season",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Models");

            migrationBuilder.AddColumn<string>(
                name: "CareInstructions",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Composition",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Line",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Product_Model",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Model", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Model_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Model_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_Model_ModelId",
                table: "Product_Model",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Model_ProductId",
                table: "Product_Model",
                column: "ProductId");
        }
    }
}
