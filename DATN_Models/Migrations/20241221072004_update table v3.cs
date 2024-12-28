using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class updatetablev3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDetail",
                table: "Tickets",
                newName: "OrderDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "Tickets",
                newName: "OrderDetail");
        }
    }
}
