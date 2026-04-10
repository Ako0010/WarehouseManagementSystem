using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class stockMovementAgainUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FromLocationId",
                table: "StockMovements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToLocationId",
                table: "StockMovements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_FromLocationId",
                table: "StockMovements",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ToLocationId",
                table: "StockMovements",
                column: "ToLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Locations_FromLocationId",
                table: "StockMovements",
                column: "FromLocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Locations_ToLocationId",
                table: "StockMovements",
                column: "ToLocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_Locations_FromLocationId",
                table: "StockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_Locations_ToLocationId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovements_FromLocationId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovements_ToLocationId",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "FromLocationId",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "ToLocationId",
                table: "StockMovements");
        }
    }
}
