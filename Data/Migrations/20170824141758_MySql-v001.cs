using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Casperinc.MainSite.API.Data.Migrations
{
    public partial class MySqlv001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NarrativeData",
                columns: table => new
                {
                    UniqueId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BodyHtml = table.Column<string>(type: "longtext", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    DisplaySequence = table.Column<short>(type: "smallint", nullable: true),
                    GuidId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    UserId = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarrativeData", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "TagData",
                columns: table => new
                {
                    UniqueId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GuidId = table.Column<Guid>(type: "char(36)", nullable: false),
                    KeyWord = table.Column<string>(type: "varchar(127)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagData", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "CommentData",
                columns: table => new
                {
                    UniqueId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GuidId = table.Column<Guid>(type: "char(36)", nullable: false),
                    NarrativeId = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Text = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    UserId = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentData", x => x.UniqueId);
                    table.ForeignKey(
                        name: "FK_CommentData_NarrativeData_NarrativeId",
                        column: x => x.NarrativeId,
                        principalTable: "NarrativeData",
                        principalColumn: "UniqueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentData_CommentData_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CommentData",
                        principalColumn: "UniqueId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NarrativeTagCrossWalk",
                columns: table => new
                {
                    NarrativeId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarrativeTagCrossWalk", x => new { x.NarrativeId, x.TagId });
                    table.ForeignKey(
                        name: "FK_NarrativeTagCrossWalk_NarrativeData_NarrativeId",
                        column: x => x.NarrativeId,
                        principalTable: "NarrativeData",
                        principalColumn: "UniqueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NarrativeTagCrossWalk_TagData_TagId",
                        column: x => x.TagId,
                        principalTable: "TagData",
                        principalColumn: "UniqueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentData_GuidId",
                table: "CommentData",
                column: "GuidId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentData_NarrativeId",
                table: "CommentData",
                column: "NarrativeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentData_ParentId",
                table: "CommentData",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_NarrativeData_GuidId",
                table: "NarrativeData",
                column: "GuidId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NarrativeTagCrossWalk_TagId",
                table: "NarrativeTagCrossWalk",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TagData_GuidId",
                table: "TagData",
                column: "GuidId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagData_KeyWord",
                table: "TagData",
                column: "KeyWord",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentData");

            migrationBuilder.DropTable(
                name: "NarrativeTagCrossWalk");

            migrationBuilder.DropTable(
                name: "NarrativeData");

            migrationBuilder.DropTable(
                name: "TagData");
        }
    }
}
