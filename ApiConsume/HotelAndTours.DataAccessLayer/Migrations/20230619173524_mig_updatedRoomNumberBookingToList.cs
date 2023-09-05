using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAndTours.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_updatedRoomNumberBookingToList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomNumberId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomNumberId",
                table: "Bookings",
                column: "RoomNumberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomNumberId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomNumberId",
                table: "Bookings",
                column: "RoomNumberId",
                unique: true);
        }
    }
}
