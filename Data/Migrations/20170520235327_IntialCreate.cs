using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainSiteCore.Data.Migrations
{
    public partial class IntialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NarrativeData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BodyHtml = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarrativeData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagData",
                columns: table => new
                {
                    KeyWord = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagData", x => x.KeyWord);
                });

            migrationBuilder.CreateTable(
                name: "NarrativeTagDataModel",
                columns: table => new
                {
                    NarrativeId = table.Column<int>(nullable: false),
                    TagKeyWord = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarrativeTagDataModel", x => new { x.NarrativeId, x.TagKeyWord });
                    table.ForeignKey(
                        name: "FK_NarrativeTagDataModel_NarrativeData_NarrativeId",
                        column: x => x.NarrativeId,
                        principalTable: "NarrativeData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NarrativeTagDataModel_TagData_TagKeyWord",
                        column: x => x.TagKeyWord,
                        principalTable: "TagData",
                        principalColumn: "KeyWord",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NarrativeTagDataModel_TagKeyWord",
                table: "NarrativeTagDataModel",
                column: "TagKeyWord");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NarrativeTagDataModel");

            migrationBuilder.DropTable(
                name: "NarrativeData");

            migrationBuilder.DropTable(
                name: "TagData");
        }
    }
}
