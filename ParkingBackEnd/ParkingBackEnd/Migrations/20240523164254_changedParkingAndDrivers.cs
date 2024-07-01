using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class changedParkingAndDrivers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParkingNumber",
                table: "ParkingAndDrivers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingNumber",
                table: "ParkingAndDrivers");
        }
    }
}
