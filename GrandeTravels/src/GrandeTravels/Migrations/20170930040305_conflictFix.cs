using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrandeTravels.Migrations
{
    public partial class conflictFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblPackage_TblShoppingCart_ShoppingCartID",
                table: "TblPackage");

            migrationBuilder.AlterColumn<int>(
                name: "ShoppingCartID",
                table: "TblPackage",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TblPackage_TblShoppingCart_ShoppingCartID",
                table: "TblPackage",
                column: "ShoppingCartID",
                principalTable: "TblShoppingCart",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblPackage_TblShoppingCart_ShoppingCartID",
                table: "TblPackage");

            migrationBuilder.AlterColumn<int>(
                name: "ShoppingCartID",
                table: "TblPackage",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_TblPackage_TblShoppingCart_ShoppingCartID",
                table: "TblPackage",
                column: "ShoppingCartID",
                principalTable: "TblShoppingCart",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
