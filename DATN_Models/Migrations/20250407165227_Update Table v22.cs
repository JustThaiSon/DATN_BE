using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablev22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the LogoUrl column if it does not exist
            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "MembershipBenefit",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            // Alter the LogoUrl column to have the desired properties
            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "MembershipBenefit",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "MembershipBenefit",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            // Drop the LogoUrl column if it was added
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "MembershipBenefit");
        }
    }
}
