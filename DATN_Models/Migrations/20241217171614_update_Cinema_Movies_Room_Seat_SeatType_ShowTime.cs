using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class update_Cinema_Movies_Room_Seat_SeatType_ShowTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SeatTypes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SeatTypes");

            migrationBuilder.DropColumn(
                name: "SeatsCount",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "SeaTypeId",
                table: "Seats",
                newName: "SeatTypeId");

            migrationBuilder.RenameColumn(
                name: "NameSeat",
                table: "Seats",
                newName: "SeatName");

            migrationBuilder.AddColumn<decimal>(
                name: "Multiplier",
                table: "ShowTimes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SeatTypeName",
                table: "SeatTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ColNumber",
                table: "Rooms",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowNumber",
                table: "Rooms",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePriceMultiplier",
                table: "Movies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Cinemas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Multiplier",
                table: "ShowTimes");

            migrationBuilder.DropColumn(
                name: "SeatTypeName",
                table: "SeatTypes");

            migrationBuilder.DropColumn(
                name: "ColNumber",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RowNumber",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "BasePriceMultiplier",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "SeatTypeId",
                table: "Seats",
                newName: "SeaTypeId");

            migrationBuilder.RenameColumn(
                name: "SeatName",
                table: "Seats",
                newName: "NameSeat");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SeatTypes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SeatTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Seats",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "SeatsCount",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Cinemas",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }
    }
}
