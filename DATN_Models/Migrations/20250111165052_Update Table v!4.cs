using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablev4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Tickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "Tickets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
