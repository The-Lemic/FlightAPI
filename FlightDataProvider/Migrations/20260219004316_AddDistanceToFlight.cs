using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDataProvider.Migrations
{
    /// <inheritdoc />
    public partial class AddDistanceToFlight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "Flights",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Flights");
        }
    }
}
