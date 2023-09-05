using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAndTours.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_hotelCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotelCategories",
                columns: table => new
                {
                    HotelCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelCategories", x => x.HotelCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "RSHotelCategories",
                columns: table => new
                {
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    HotelCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSHotelCategories", x => new { x.HotelId, x.HotelCategoryId });
                    table.ForeignKey(
                        name: "FK_RSHotelCategories_HotelCategories_HotelCategoryId",
                        column: x => x.HotelCategoryId,
                        principalTable: "HotelCategories",
                        principalColumn: "HotelCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RSHotelCategories_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "HotelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RSHotelCategories_HotelCategoryId",
                table: "RSHotelCategories",
                column: "HotelCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RSHotelCategories");

            migrationBuilder.DropTable(
                name: "HotelCategories");
        }
    }
}
