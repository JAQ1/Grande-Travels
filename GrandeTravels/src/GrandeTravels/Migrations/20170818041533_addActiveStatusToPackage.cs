using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrandeTravels.Migrations
{
    public partial class addActiveStatusToPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TblPackage",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "TblPackage",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TblPackage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActiveStatus",
                table: "TblPackage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveStatus",
                table: "TblPackage");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TblPackage",
                maxLength: 50,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "TblPackage",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TblPackage",
                maxLength: 225,
                nullable: false);
        }
    }
}
