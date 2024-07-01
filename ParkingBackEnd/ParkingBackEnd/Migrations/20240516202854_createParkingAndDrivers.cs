using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class createParkingAndDrivers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingAndDrivers",
                columns: table => new
                {
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    ParkingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingAndDrivers", x => new { x.DriverId, x.ParkingId });
                    table.ForeignKey(
                        name: "FK_ParkingAndDrivers_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingAndDrivers_Parkings_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "Parkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingAndDrivers_ParkingId",
                table: "ParkingAndDrivers",
                column: "ParkingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingAndDrivers");
        }
    }
}
