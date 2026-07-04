using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAI.Migrations
{
    /// <inheritdoc />
    public partial class new_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_UserId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_UserId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TravelType",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "DayNumber",
                table: "TripDestinations",
                newName: "Sequence");

            migrationBuilder.AddColumn<int>(
                name: "DestinationId",
                table: "Itineraries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DestinationFeature",
                columns: table => new
                {
                    DestinationFeatureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinationId = table.Column<int>(type: "int", nullable: false),
                    Culture = table.Column<int>(type: "int", nullable: false),
                    Adventure = table.Column<int>(type: "int", nullable: false),
                    Nature = table.Column<int>(type: "int", nullable: false),
                    Luxury = table.Column<int>(type: "int", nullable: false),
                    Wildlife = table.Column<int>(type: "int", nullable: false),
                    Trekking = table.Column<int>(type: "int", nullable: false),
                    Family = table.Column<int>(type: "int", nullable: false),
                    Relaxation = table.Column<int>(type: "int", nullable: false),
                    Religious = table.Column<int>(type: "int", nullable: false),
                    NightLife = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinationFeature", x => x.DestinationFeatureId);
                    table.ForeignKey(
                        name: "FK_DestinationFeature_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "DestinationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DestinationFeature_DestinationId",
                table: "DestinationFeature",
                column: "DestinationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DestinationFeature");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                table: "Itineraries");

            migrationBuilder.RenameColumn(
                name: "Sequence",
                table: "TripDestinations",
                newName: "DayNumber");

            migrationBuilder.AddColumn<string>(
                name: "TravelType",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_UserId",
                table: "Trips",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_UserId",
                table: "Trips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
