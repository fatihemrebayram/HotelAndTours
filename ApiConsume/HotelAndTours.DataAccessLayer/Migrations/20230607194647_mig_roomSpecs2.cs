using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAndTours.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_roomSpecs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomSpecs_Rooms_RoomId",
                table: "RoomSpecs");

            migrationBuilder.DropIndex(
                name: "IX_RoomSpecs_RoomId",
                table: "RoomSpecs");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "RoomSpecs");

            migrationBuilder.CreateTable(
                name: "RRSpecs",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    RoomSpecsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RRSpecs", x => new { x.RoomId, x.RoomSpecsId });
                    table.ForeignKey(
                        name: "FK_RRSpecs_RoomSpecs_RoomSpecsId",
                        column: x => x.RoomSpecsId,
                        principalTable: "RoomSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RRSpecs_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RRSpecs_RoomSpecsId",
                table: "RRSpecs",
                column: "RoomSpecsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RRSpecs");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "RoomSpecs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RoomSpecs_RoomId",
                table: "RoomSpecs",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomSpecs_Rooms_RoomId",
                table: "RoomSpecs",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
