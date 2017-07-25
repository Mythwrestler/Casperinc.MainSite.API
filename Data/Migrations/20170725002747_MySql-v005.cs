using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CasperInc.MainSite.API.Data.Migrations
{
    public partial class MySqlv005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NarrativeData",
                nullable: false,
                oldClrType: typeof(Guid))
                .OldAnnotation("MySql:ValueGeneratedOnAdd", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NarrativeData",
                nullable: false,
                oldClrType: typeof(Guid))
                .Annotation("MySql:ValueGeneratedOnAdd", true);
        }
    }
}
