using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrandeTravels.Migrations
{
    public partial class uptBookingClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TblBooking");

            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "TblBooking",
                newName: "DepartureDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDate",
                table: "TblBooking",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalDate",
                table: "TblBooking");

            migrationBuilder.RenameColumn(
                name: "DepartureDate",
                table: "TblBooking",
                newName: "BookingDate");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TblBooking",
                nullable: true);
        }
    }
}
