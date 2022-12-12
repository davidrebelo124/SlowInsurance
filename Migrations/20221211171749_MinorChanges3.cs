using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowInsurance.Migrations
{
    /// <inheritdoc />
    public partial class MinorChanges3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssuedDate",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuedDate",
                table: "Invoice");
        }
    }
}
