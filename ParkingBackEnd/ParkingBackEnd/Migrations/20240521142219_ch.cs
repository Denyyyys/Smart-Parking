using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class ch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxTimeInSeconds",
                table: "ParkingHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxTimeInSeconds",
                table: "ParkingHistory");
        }
    }
}
