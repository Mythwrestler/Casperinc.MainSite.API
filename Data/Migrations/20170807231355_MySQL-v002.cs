using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CasperInc.MainSite.API.Data.Migrations
{
    public partial class MySQLv002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NarrativeData_AspNetUsers_UserId",
                table: "NarrativeData");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NarrativeData",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "DisplaySequence",
                table: "NarrativeData",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AddForeignKey(
                name: "FK_NarrativeData_AspNetUsers_UserId",
                table: "NarrativeData",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NarrativeData_AspNetUsers_UserId",
                table: "NarrativeData");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NarrativeData",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<short>(
                name: "DisplaySequence",
                table: "NarrativeData",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NarrativeData_AspNetUsers_UserId",
                table: "NarrativeData",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
