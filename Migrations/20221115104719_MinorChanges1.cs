using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowInsurance.Migrations
{
    /// <inheritdoc />
    public partial class MinorChanges1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentSchedule",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "RegisterDate",
                table: "Vehicle",
                newName: "RegistrationDate");

            migrationBuilder.AddColumn<string>(
                name: "Validity",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validity",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                table: "Vehicle",
                newName: "RegisterDate");

            migrationBuilder.AddColumn<string>(
                name: "PaymentSchedule",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
