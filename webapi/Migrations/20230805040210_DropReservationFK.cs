using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class DropReservationFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Customer",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Staff",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_table",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_customer_id",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_staff_id",
                table: "Reservation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reservation_customer_id",
                table: "Reservation",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_staff_id",
                table: "Reservation",
                column: "staff_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Customer",
                table: "Reservation",
                column: "customer_id",
                principalTable: "Customer",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Staff",
                table: "Reservation",
                column: "staff_id",
                principalTable: "Staff",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_table",
                table: "Reservation",
                column: "tableNo",
                principalTable: "table",
                principalColumn: "tableNo");
        }
    }
}
