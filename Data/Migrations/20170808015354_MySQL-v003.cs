using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CasperInc.MainSite.API.Data.Migrations
{
    public partial class MySQLv003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentData_NarrativeData_NarrativeId",
                table: "CommentData");

            migrationBuilder.AlterColumn<long>(
                name: "NarrativeId",
                table: "CommentData",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentData_NarrativeData_NarrativeId",
                table: "CommentData",
                column: "NarrativeId",
                principalTable: "NarrativeData",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentData_NarrativeData_NarrativeId",
                table: "CommentData");

            migrationBuilder.AlterColumn<long>(
                name: "NarrativeId",
                table: "CommentData",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_CommentData_NarrativeData_NarrativeId",
                table: "CommentData",
                column: "NarrativeId",
                principalTable: "NarrativeData",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
