using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowInsurance.Migrations
{
    /// <inheritdoc />
    public partial class MinorChanges4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Accident",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Accident");
        }
    }
}
