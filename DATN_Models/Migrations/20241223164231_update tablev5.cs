using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class updatetablev5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Orders",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "OrderDetails",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<string>(
                name: "TickeCode",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Services",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TickeCode",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Orders",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "OrderDetails",
                newName: "CreateDate");
        }
    }
}
