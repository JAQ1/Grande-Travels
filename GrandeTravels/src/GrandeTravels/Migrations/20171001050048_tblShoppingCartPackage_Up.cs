using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GrandeTravels.Migrations
{
    public partial class tblShoppingCartPackage_Up : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblPackage_TblShoppingCart_ShoppingCartID",
                table: "TblPackage");

            migrationBuilder.DropIndex(
                name: "IX_TblPackage_ShoppingCartID",
                table: "TblPackage");

            migrationBuilder.DropColumn(
                name: "ShoppingCartID",
                table: "TblPackage");

            migrationBuilder.AddColumn<bool>(
                name: "CheckedOut",
                table: "TblShoppingCart",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TblShoppingCartPackage",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PackageID = table.Column<int>(nullable: false),
                    ShoppingCartID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblShoppingCartPackage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TblShoppingCartPackage_TblPackage_PackageID",
                        column: x => x.PackageID,
                        principalTable: "TblPackage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblShoppingCartPackage_TblShoppingCart_ShoppingCartID",
                        column: x => x.ShoppingCartID,
                        principalTable: "TblShoppingCart",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblShoppingCartPackage_PackageID",
                table: "TblShoppingCartPackage",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_TblShoppingCartPackage_ShoppingCartID",
                table: "TblShoppingCartPackage",
                column: "ShoppingCartID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblShoppingCartPackage");

            migrationBuilder.DropColumn(
                name: "CheckedOut",
                table: "TblShoppingCart");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartID",
                table: "TblPackage",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblPackage_ShoppingCartID",
                table: "TblPackage",
                column: "ShoppingCartID");

            migrationBuilder.AddForeignKey(
                name: "FK_TblPackage_TblShoppingCart_ShoppingCartID",
                table: "TblPackage",
                column: "ShoppingCartID",
                principalTable: "TblShoppingCart",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
