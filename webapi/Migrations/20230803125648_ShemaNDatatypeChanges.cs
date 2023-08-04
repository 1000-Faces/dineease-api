using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class ShemaNDatatypeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "User",
                schema: "userMGT",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Staff",
                schema: "userMGT",
                newName: "Staff");

            migrationBuilder.RenameTable(
                name: "Role",
                schema: "userMGT",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "Customer",
                schema: "userMGT",
                newName: "Customer");

            migrationBuilder.RenameTable(
                name: "Authentication",
                schema: "userMGT",
                newName: "Authentication");

            migrationBuilder.DropColumn(
                name: "last_updated",
                table: "Authentication");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_updated",
                table: "Authentication",
                type: "datetime",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_logged",
                table: "Authentication",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "User",
                newName: "User",
                newSchema: "userMGT");

            migrationBuilder.RenameTable(
                name: "Staff",
                newName: "Staff",
                newSchema: "userMGT");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Role",
                newSchema: "userMGT");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customer",
                newSchema: "userMGT");

            migrationBuilder.RenameTable(
                name: "Authentication",
                newName: "Authentication",
                newSchema: "userMGT");

            migrationBuilder.DropColumn(
                name: "last_updated",
                schema: "userMGT",
                table: "Authentication");

            migrationBuilder.AddColumn<byte[]>(
                name: "last_updated",
                schema: "userMGT",
                table: "Authentication",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_logged",
                schema: "userMGT",
                table: "Authentication",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
