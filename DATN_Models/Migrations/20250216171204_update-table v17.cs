using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class updatetablev17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChangeLog",
                table: "ChangeLog");

            migrationBuilder.RenameTable(
                name: "ChangeLog",
                newName: "ChangeLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChangeLogs",
                table: "ChangeLogs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChangeLogs",
                table: "ChangeLogs");

            migrationBuilder.RenameTable(
                name: "ChangeLogs",
                newName: "ChangeLog");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChangeLog",
                table: "ChangeLog",
                column: "Id");
        }
    }
}
