using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class changedParkingAndDriverss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParkingNumber",
                table: "ParkingAndDrivers",
                newName: "ParkingPlaceNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParkingPlaceNumber",
                table: "ParkingAndDrivers",
                newName: "ParkingNumber");
        }
    }
}
