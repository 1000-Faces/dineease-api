using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReserveidType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation");

            migrationBuilder.AddColumn<Guid>(
                name: "reservation_id_temp",
                table: "Reservation",
                type: "uniqueidentifier",
                maxLength: 10,
                nullable: false);

            migrationBuilder.DropColumn(
                name: "reservation_id",
                table: "Reservation");

            migrationBuilder.RenameColumn(
                name: "reservation_id_temp",
                table: "Reservation",
                newName: "reservation_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation",
                column: "reservation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "reservation_id",
                table: "Reservation",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 10);
        }
    }
}
