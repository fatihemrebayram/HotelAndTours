using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAndTours.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_updated_HotelandRooms_Added_Lists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId",
                table: "Rooms",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomNumbers_RoomId",
                table: "RoomNumbers",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomNumbers_Rooms_RoomId",
                table: "RoomNumbers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Hotels_HotelId",
                table: "Rooms",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomNumbers_Rooms_RoomId",
                table: "RoomNumbers");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Hotels_HotelId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_HotelId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_RoomNumbers_RoomId",
                table: "RoomNumbers");
        }
    }
}
