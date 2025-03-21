using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class up6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_seatStatusByShowTimes",
                table: "seatStatusByShowTimes");

            migrationBuilder.RenameTable(
                name: "seatStatusByShowTimes",
                newName: "SeatByShowTimes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeatByShowTimes",
                table: "SeatByShowTimes",
                column: "SeatStatusByShowTimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeatByShowTimes",
                table: "SeatByShowTimes");

            migrationBuilder.RenameTable(
                name: "SeatByShowTimes",
                newName: "seatStatusByShowTimes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_seatStatusByShowTimes",
                table: "seatStatusByShowTimes",
                column: "SeatStatusByShowTimeId");
        }
    }
}
