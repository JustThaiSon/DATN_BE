using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class Nghia_Update_MovieFormat_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceMultiplier",
                table: "MovieFormats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceMultiplier",
                table: "MovieFormats",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 1.0m);
        }
    }
}
