using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowInsurance.Migrations
{
    /// <inheritdoc />
    public partial class RelationChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accident_Vehicle_VehicleEntityId",
                table: "Accident");

            migrationBuilder.DropIndex(
                name: "IX_Accident_VehicleEntityId",
                table: "Accident");

            migrationBuilder.DropColumn(
                name: "VehicleEntityId",
                table: "Accident");

            migrationBuilder.CreateTable(
                name: "AccidentEntityVehicleEntity",
                columns: table => new
                {
                    AccidentsId = table.Column<int>(type: "int", nullable: false),
                    VehiclesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccidentEntityVehicleEntity", x => new { x.AccidentsId, x.VehiclesId });
                    table.ForeignKey(
                        name: "FK_AccidentEntityVehicleEntity_Accident_AccidentsId",
                        column: x => x.AccidentsId,
                        principalTable: "Accident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccidentEntityVehicleEntity_Vehicle_VehiclesId",
                        column: x => x.VehiclesId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccidentEntityVehicleEntity_VehiclesId",
                table: "AccidentEntityVehicleEntity",
                column: "VehiclesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccidentEntityVehicleEntity");

            migrationBuilder.AddColumn<int>(
                name: "VehicleEntityId",
                table: "Accident",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accident_VehicleEntityId",
                table: "Accident",
                column: "VehicleEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accident_Vehicle_VehicleEntityId",
                table: "Accident",
                column: "VehicleEntityId",
                principalTable: "Vehicle",
                principalColumn: "Id");
        }
    }
}
