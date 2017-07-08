using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CasperInc.MainSite.API.Data.Migrations
{
    public partial class MySQLv001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NarrativeData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
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
                    Id = table.Column<Guid>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    KeyWord = table.Column<string>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NarrativeTagCrossWalk",
                columns: table => new
                {
                    NarrativeId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarrativeTagCrossWalk", x => new { x.NarrativeId, x.TagId });
                    table.ForeignKey(
                        name: "FK_NarrativeTagCrossWalk_NarrativeData_NarrativeId",
                        column: x => x.NarrativeId,
                        principalTable: "NarrativeData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NarrativeTagCrossWalk_TagData_TagId",
                        column: x => x.TagId,
                        principalTable: "TagData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NarrativeTagCrossWalk_TagId",
                table: "NarrativeTagCrossWalk",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TagData_KeyWord",
                table: "TagData",
                column: "KeyWord",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NarrativeTagCrossWalk");

            migrationBuilder.DropTable(
                name: "NarrativeData");

            migrationBuilder.DropTable(
                name: "TagData");
        }
    }
}
