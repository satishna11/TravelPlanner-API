using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAI.Migrations
{
    /// <inheritdoc />
    public partial class LocalChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Family",
                table: "DestinationFeatures");

            migrationBuilder.DropColumn(
                name: "NightLife",
                table: "DestinationFeatures");

            migrationBuilder.DropColumn(
                name: "Relaxation",
                table: "DestinationFeatures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Family",
                table: "DestinationFeatures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NightLife",
                table: "DestinationFeatures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Relaxation",
                table: "DestinationFeatures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
