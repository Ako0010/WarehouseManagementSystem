using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class transferFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Transfers_FromLocationId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Transfers_ToLocationId",
                table: "Transfers");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Locations_FromLocationId",
                table: "Transfers",
                column: "FromLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Locations_ToLocationId",
                table: "Transfers",
                column: "ToLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Locations_FromLocationId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Locations_ToLocationId",
                table: "Transfers");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Transfers_FromLocationId",
                table: "Transfers",
                column: "FromLocationId",
                principalTable: "Transfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Transfers_ToLocationId",
                table: "Transfers",
                column: "ToLocationId",
                principalTable: "Transfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
