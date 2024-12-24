using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class updatedatav8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "PricingRules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecialDay",
                table: "PricingRules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecialMonth",
                table: "PricingRules",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "PricingRules");

            migrationBuilder.DropColumn(
                name: "SpecialDay",
                table: "PricingRules");

            migrationBuilder.DropColumn(
                name: "SpecialMonth",
                table: "PricingRules");
        }
    }
}
