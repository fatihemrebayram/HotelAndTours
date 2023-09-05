using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAndTours.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_connected_Tables_update5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_RoomNumbers_RoomNumberId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomNumberId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomNumberId",
                table: "Bookings",
                column: "RoomNumberId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_RoomNumbers_RoomNumberId",
                table: "Bookings",
                column: "RoomNumberId",
                principalTable: "RoomNumbers",
                principalColumn: "RoomNumberId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
