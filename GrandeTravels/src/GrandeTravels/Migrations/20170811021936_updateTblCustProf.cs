using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrandeTravels.Migrations
{
    public partial class updateTblCustProf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TblCustProfile",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "CustomerProfileId",
                table: "TblCustProfile",
                newName: "CustomerProfileID");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "TblCustProfile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayPhotoPath",
                table: "TblCustProfile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TblCustProfile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "TblCustProfile");

            migrationBuilder.DropColumn(
                name: "DisplayPhotoPath",
                table: "TblCustProfile");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "TblCustProfile");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "TblCustProfile",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CustomerProfileID",
                table: "TblCustProfile",
                newName: "CustomerProfileId");
        }
    }
}
