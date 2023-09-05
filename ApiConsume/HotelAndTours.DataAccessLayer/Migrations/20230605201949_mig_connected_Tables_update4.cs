using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAndTours.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_connected_Tables_update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomRoomNumber");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomNumbers_Rooms_RoomId",
                table: "RoomNumbers");

            migrationBuilder.DropIndex(
                name: "IX_RoomNumbers_RoomId",
                table: "RoomNumbers");

            migrationBuilder.CreateTable(
                name: "RoomRoomNumber",
                columns: table => new
                {
                    RoomNumbersRoomNumberId = table.Column<int>(type: "int", nullable: false),
                    RoomsRoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRoomNumber", x => new { x.RoomNumbersRoomNumberId, x.RoomsRoomId });
                    table.ForeignKey(
                        name: "FK_RoomRoomNumber_RoomNumbers_RoomNumbersRoomNumberId",
                        column: x => x.RoomNumbersRoomNumberId,
                        principalTable: "RoomNumbers",
                        principalColumn: "RoomNumberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomRoomNumber_Rooms_RoomsRoomId",
                        column: x => x.RoomsRoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRoomNumber_RoomsRoomId",
                table: "RoomRoomNumber",
                column: "RoomsRoomId");
        }
    }
}
