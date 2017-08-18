using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrandeTravels.Migrations
{
    public partial class removeImageCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageImage",
                table: "TblPackage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PackageImage",
                table: "TblPackage",
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
