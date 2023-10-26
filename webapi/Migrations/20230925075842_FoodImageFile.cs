using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Local.Migrations
{
    /// <inheritdoc />
    public partial class FoodImageFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodOrders");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Reservation",
                type: "nchar(50)",
                fixedLength: true,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "order_status",
                table: "orders",
                type: "nchar(50)",
                fixedLength: true,
                maxLength: 50,
                nullable: false,
                defaultValueSql: "(N'pending')",
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "Order_Foods",
                type: "int",
                nullable: true,
                defaultValueSql: "((1))");

            migrationBuilder.AlterColumn<Guid>(
                name: "meal_id",
                table: "Meal_promotion",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "total_price",
                table: "Meal_foods",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "Meal_foods",
                type: "int",
                nullable: true,
                defaultValueSql: "((1))",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "meal_id",
                table: "Meal_foods",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "meal_name",
                table: "Meal",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "Custom",
                table: "Meal",
                type: "bit",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "((1))");

            migrationBuilder.AlterColumn<Guid>(
                name: "meal_id",
                table: "Meal",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "foodImg",
                table: "Food",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "paymentMethod",
                table: "checkout",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                defaultValueSql: "(N'cash')",
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "chat",
                columns: table => new
                {
                    MessageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceiverID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Message = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat", x => x.MessageID);
                });

            migrationBuilder.CreateTable(
                name: "Order_Meal",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    meal_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Meal", x => new { x.order_id, x.meal_id });
                    table.ForeignKey(
                        name: "FK_Order_Meal_Meal",
                        column: x => x.meal_id,
                        principalTable: "Meal",
                        principalColumn: "meal_id");
                    table.ForeignKey(
                        name: "FK_Order_Meal_orders",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_Meal_meal_id",
                table: "Order_Meal",
                column: "meal_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat");

            migrationBuilder.DropTable(
                name: "Order_Meal");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "Order_Foods");

            migrationBuilder.DropColumn(
                name: "foodImg",
                table: "Food");

            migrationBuilder.AlterColumn<string>(
                name: "order_status",
                table: "orders",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(50)",
                oldFixedLength: true,
                oldMaxLength: 50,
                oldDefaultValueSql: "(N'pending')");

            migrationBuilder.AlterColumn<int>(
                name: "meal_id",
                table: "Meal_promotion",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<double>(
                name: "total_price",
                table: "Meal_foods",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "Meal_foods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "((1))");

            migrationBuilder.AlterColumn<int>(
                name: "meal_id",
                table: "Meal_foods",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "meal_name",
                table: "Meal",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Custom",
                table: "Meal",
                type: "bit",
                nullable: true,
                defaultValueSql: "((1))",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<int>(
                name: "meal_id",
                table: "Meal",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "paymentMethod",
                table: "checkout",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValueSql: "(N'cash')");

            migrationBuilder.CreateTable(
                name: "FoodOrders",
                columns: table => new
                {
                    FoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodOrders", x => new { x.FoodId, x.OrderId });
                });
        }
    }
}
