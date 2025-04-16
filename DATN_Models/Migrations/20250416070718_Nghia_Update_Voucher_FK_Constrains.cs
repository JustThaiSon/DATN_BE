using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Models.Migrations
{
    /// <inheritdoc />
    public partial class Nghia_Update_Voucher_FK_Constrains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Vouchers_VoucherId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherUsages_AppUsers_UserId",
                table: "VoucherUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherUsages_Orders_OrderId",
                table: "VoucherUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherUsages_Vouchers_VoucherId",
                table: "VoucherUsages");

            migrationBuilder.DropIndex(
                name: "IX_VoucherUsages_OrderId",
                table: "VoucherUsages");

            migrationBuilder.DropIndex(
                name: "IX_VoucherUsages_UserId",
                table: "VoucherUsages");

            migrationBuilder.DropIndex(
                name: "IX_VoucherUsages_VoucherId",
                table: "VoucherUsages");

            migrationBuilder.DropIndex(
                name: "IX_Orders_VoucherId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VoucherUsages_OrderId",
                table: "VoucherUsages",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherUsages_UserId",
                table: "VoucherUsages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherUsages_VoucherId",
                table: "VoucherUsages",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VoucherId",
                table: "Orders",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Vouchers_VoucherId",
                table: "Orders",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherUsages_AppUsers_UserId",
                table: "VoucherUsages",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherUsages_Orders_OrderId",
                table: "VoucherUsages",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherUsages_Vouchers_VoucherId",
                table: "VoucherUsages",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
