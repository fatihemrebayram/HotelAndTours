using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAndTours.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_Booking_added_addteddate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "Bookings");
        }
    }
}
