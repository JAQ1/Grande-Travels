using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrandeTravels.Migrations
{
    public partial class fixRelationTblProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblFeedback_AspNetUsers_UserId",
                table: "TblFeedback");

            migrationBuilder.DropIndex(
                name: "IX_TblFeedback_UserId",
                table: "TblFeedback");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TblFeedback");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TblFeedback",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblFeedback_UserId",
                table: "TblFeedback",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblFeedback_AspNetUsers_UserId",
                table: "TblFeedback",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
