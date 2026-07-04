using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAI.Migrations
{
    /// <inheritdoc />
    public partial class new_updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DestinationFeature_Destinations_DestinationId",
                table: "DestinationFeature");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DestinationFeature",
                table: "DestinationFeature");

            migrationBuilder.RenameTable(
                name: "DestinationFeature",
                newName: "DestinationFeatures");

            migrationBuilder.RenameIndex(
                name: "IX_DestinationFeature_DestinationId",
                table: "DestinationFeatures",
                newName: "IX_DestinationFeatures_DestinationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DestinationFeatures",
                table: "DestinationFeatures",
                column: "DestinationFeatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_DestinationFeatures_Destinations_DestinationId",
                table: "DestinationFeatures",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "DestinationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DestinationFeatures_Destinations_DestinationId",
                table: "DestinationFeatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DestinationFeatures",
                table: "DestinationFeatures");

            migrationBuilder.RenameTable(
                name: "DestinationFeatures",
                newName: "DestinationFeature");

            migrationBuilder.RenameIndex(
                name: "IX_DestinationFeatures_DestinationId",
                table: "DestinationFeature",
                newName: "IX_DestinationFeature_DestinationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DestinationFeature",
                table: "DestinationFeature",
                column: "DestinationFeatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_DestinationFeature_Destinations_DestinationId",
                table: "DestinationFeature",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "DestinationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
