using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class updatedata_v2_add_PricingRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Multiplier",
                table: "ShowTimes");

            migrationBuilder.DropColumn(
                name: "BasePriceMultiplier",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "SeatTypes",
                newName: "Multiplier");

            migrationBuilder.RenameColumn(
                name: "RowNumber",
                table: "Rooms",
                newName: "TotalRowNumber");

            migrationBuilder.RenameColumn(
                name: "ColNumber",
                table: "Rooms",
                newName: "TotalColNumber");

            migrationBuilder.AddColumn<int>(
                name: "ColNumber",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowNumber",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "SeatPrice",
                table: "Seats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PricingRules",
                columns: table => new
                {
                    PricingRuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RuleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Multiplier = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingRules", x => x.PricingRuleId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PricingRules");

            migrationBuilder.DropColumn(
                name: "ColNumber",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "RowNumber",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatPrice",
                table: "Seats");

            migrationBuilder.RenameColumn(
                name: "Multiplier",
                table: "SeatTypes",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "TotalRowNumber",
                table: "Rooms",
                newName: "RowNumber");

            migrationBuilder.RenameColumn(
                name: "TotalColNumber",
                table: "Rooms",
                newName: "ColNumber");

            migrationBuilder.AddColumn<decimal>(
                name: "Multiplier",
                table: "ShowTimes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePriceMultiplier",
                table: "Movies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
