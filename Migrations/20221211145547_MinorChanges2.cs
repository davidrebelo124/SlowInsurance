using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowInsurance.Migrations
{
    /// <inheritdoc />
    public partial class MinorChanges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Val",
                table: "Invoice",
                newName: "ExpirationDate");

            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Invoice",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Plate",
                table: "Vehicle",
                column: "Plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicle_Plate",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "Invoice",
                newName: "Val");

            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<float>(
                name: "Value",
                table: "Invoice",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
