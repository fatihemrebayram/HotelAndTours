using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAndTours.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_hotelUpdatecategoriesId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HotelCategoriesIds",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotelCategoriesIds",
                table: "Hotels");
        }
    }
}
