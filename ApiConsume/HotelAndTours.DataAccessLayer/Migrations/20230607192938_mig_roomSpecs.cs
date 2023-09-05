using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAndTours.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_roomSpecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomSpec = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomSpecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomSpecs_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomNumberId",
                table: "Bookings",
                column: "RoomNumberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomSpecs_RoomId",
                table: "RoomSpecs",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_RoomNumbers_RoomNumberId",
                table: "Bookings",
                column: "RoomNumberId",
                principalTable: "RoomNumbers",
                principalColumn: "RoomNumberId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_RoomNumbers_RoomNumberId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "RoomSpecs");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomNumberId",
                table: "Bookings");
        }
    }
}
