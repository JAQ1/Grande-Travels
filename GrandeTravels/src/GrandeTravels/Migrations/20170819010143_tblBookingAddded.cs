using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GrandeTravels.Migrations
{
    public partial class tblBookingAddded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblBooking",
                columns: table => new
                {
                    BookingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BookingDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PackageID = table.Column<int>(nullable: false),
                    PackageName = table.Column<string>(nullable: true),
                    People = table.Column<int>(nullable: false),
                    TotalCost = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblBooking", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_TblBooking_TblPackage_PackageID",
                        column: x => x.PackageID,
                        principalTable: "TblPackage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblBooking_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblBooking_PackageID",
                table: "TblBooking",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_TblBooking_UserID",
                table: "TblBooking",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblBooking");
        }
    }
}
