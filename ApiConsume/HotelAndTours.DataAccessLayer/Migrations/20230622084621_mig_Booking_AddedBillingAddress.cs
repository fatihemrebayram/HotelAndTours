using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAndTours.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_Booking_AddedBillingAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "Bookings");
        }
    }
}
