using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CasperInc.MainSite.API.Data.Migrations
{
    public partial class MySqlv004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TagData",
                nullable: false,
                oldClrType: typeof(Guid))
                .Annotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NarrativeData",
                nullable: false,
                oldClrType: typeof(Guid))
                .Annotation("MySql:ValueGeneratedOnAdd", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TagData",
                nullable: false,
                oldClrType: typeof(Guid))
                .OldAnnotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NarrativeData",
                nullable: false,
                oldClrType: typeof(Guid))
                .OldAnnotation("MySql:ValueGeneratedOnAdd", true);
        }
    }
}
