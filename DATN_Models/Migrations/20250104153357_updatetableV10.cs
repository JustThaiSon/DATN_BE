using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class updatetableV10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "ShowtimeId",
                table: "Tickets",
                newName: "SeatByShowTimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SeatByShowTimeId",
                table: "Tickets",
                newName: "ShowtimeId");

            migrationBuilder.AddColumn<Guid>(
                name: "SeatId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
