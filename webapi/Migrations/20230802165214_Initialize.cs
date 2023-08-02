using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "userMGT");

            migrationBuilder.CreateTable(
                name: "Beverage",
                columns: table => new
                {
                    food_id = table.Column<int>(type: "int", nullable: false),
                    beverage_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Dessert",
                columns: table => new
                {
                    food_id = table.Column<int>(type: "int", nullable: false),
                    dessert_id = table.Column<int>(type: "int", nullable: true),
                    served_temparature = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FoodCategory",
                columns: table => new
                {
                    categoryID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    categoryName = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_foodCategory", x => x.categoryID);
                });

            migrationBuilder.CreateTable(
                name: "FoodUser",
                columns: table => new
                {
                    FoodId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodUser", x => new { x.FoodId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    food_id = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Main_dish",
                columns: table => new
                {
                    food_id = table.Column<int>(type: "int", nullable: false),
                    main_dish_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Meal",
                columns: table => new
                {
                    meal_id = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    meal_name = table.Column<int>(type: "int", nullable: false),
                    discription = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal", x => x.meal_id);
                });

            migrationBuilder.CreateTable(
                name: "promotion",
                columns: table => new
                {
                    promotionID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    discount = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    deadline = table.Column<DateTime>(type: "date", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion", x => x.promotionID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "userMGT",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "Side_dish",
                columns: table => new
                {
                    food_id = table.Column<int>(type: "int", nullable: false),
                    side_dish_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Stater",
                columns: table => new
                {
                    food_id = table.Column<int>(type: "int", nullable: false),
                    stater_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "table",
                columns: table => new
                {
                    tableNo = table.Column<int>(type: "int", nullable: false),
                    seating = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    availability = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_table", x => x.tableNo);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "userMGT",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    user_image = table.Column<string>(type: "text", nullable: true),
                    user_image_type = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: true),
                    created_date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    food_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    food_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    categoryID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    food_type = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    availability = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food", x => x.food_id);
                    table.ForeignKey(
                        name: "FK_Food_FoodCategory",
                        column: x => x.categoryID,
                        principalTable: "FoodCategory",
                        principalColumn: "categoryID");
                });

            migrationBuilder.CreateTable(
                name: "Meal_promotion",
                columns: table => new
                {
                    meal_id = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    promotion_id = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal_promotion", x => new { x.meal_id, x.promotion_id });
                    table.ForeignKey(
                        name: "FK_Meal_promotion_Meal",
                        column: x => x.meal_id,
                        principalTable: "Meal",
                        principalColumn: "meal_id");
                    table.ForeignKey(
                        name: "FK_Meal_promotion_promotion",
                        column: x => x.promotion_id,
                        principalTable: "promotion",
                        principalColumn: "promotionID");
                });

            migrationBuilder.CreateTable(
                name: "Authentication",
                schema: "userMGT",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    password = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    salt = table.Column<byte[]>(type: "binary(50)", fixedLength: true, maxLength: 50, nullable: false),
                    role = table.Column<int>(type: "int", nullable: true),
                    last_logged = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_updated = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authentication", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Authentication_Role",
                        column: x => x.role,
                        principalSchema: "userMGT",
                        principalTable: "Role",
                        principalColumn: "role_id");
                    table.ForeignKey(
                        name: "FK_Authentication_User",
                        column: x => x.user_id,
                        principalSchema: "userMGT",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "userMGT",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    loyality_pts = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer_1", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Customer_User",
                        column: x => x.user_id,
                        principalSchema: "userMGT",
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                schema: "userMGT",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    job_title = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    is_active = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportStaff_1", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Staff_User",
                        column: x => x.user_id,
                        principalSchema: "userMGT",
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Calender_date",
                columns: table => new
                {
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    food_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calender_date", x => x.date);
                    table.ForeignKey(
                        name: "FK_Calender_date_Food",
                        column: x => x.food_id,
                        principalTable: "Food",
                        principalColumn: "food_id");
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                schema: "userMGT",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    food_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => new { x.user_id, x.food_id });
                    table.ForeignKey(
                        name: "FK_Favorites_Food",
                        column: x => x.food_id,
                        principalTable: "Food",
                        principalColumn: "food_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_User",
                        column: x => x.user_id,
                        principalSchema: "userMGT",
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Food_portions",
                columns: table => new
                {
                    food_id = table.Column<int>(type: "int", nullable: false),
                    regular_price = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    large_price = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food_portions", x => x.food_id);
                    table.ForeignKey(
                        name: "FK_Food_portions_Food",
                        column: x => x.food_id,
                        principalTable: "Food",
                        principalColumn: "food_id");
                });

            migrationBuilder.CreateTable(
                name: "Meal_foods",
                columns: table => new
                {
                    meal_id = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    food_id = table.Column<int>(type: "int", nullable: false),
                    quantity1 = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal_foods_1", x => x.meal_id);
                    table.ForeignKey(
                        name: "FK_Meal_foods_Food",
                        column: x => x.food_id,
                        principalTable: "Food",
                        principalColumn: "food_id");
                    table.ForeignKey(
                        name: "FK_Meal_foods_Meal",
                        column: x => x.meal_id,
                        principalTable: "Meal",
                        principalColumn: "meal_id");
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    reservation_id = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    staffID = table.Column<int>(type: "int", nullable: false),
                    tableNo = table.Column<int>(type: "int", nullable: false),
                    reservation_datetime = table.Column<DateTime>(type: "datetime", nullable: true),
                    departure = table.Column<DateTime>(type: "datetime", nullable: true),
                    actual_departure = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.reservation_id);
                    table.ForeignKey(
                        name: "FK_Reservation_Customer",
                        column: x => x.customer_id,
                        principalSchema: "userMGT",
                        principalTable: "Customer",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_Reservation_Staff",
                        column: x => x.staffID,
                        principalSchema: "userMGT",
                        principalTable: "Staff",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_Reservation_table",
                        column: x => x.tableNo,
                        principalTable: "table",
                        principalColumn: "tableNo");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    order_id = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    reservation_id = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    total = table.Column<double>(type: "float", nullable: false),
                    discount = table.Column<double>(type: "float", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    order_status = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_orders_Reservation",
                        column: x => x.reservation_id,
                        principalTable: "Reservation",
                        principalColumn: "reservation_id");
                });

            migrationBuilder.CreateTable(
                name: "checkout",
                columns: table => new
                {
                    orderID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    staffID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    paymentMethod = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    Amount = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkout", x => x.orderID);
                    table.ForeignKey(
                        name: "FK_checkout_orders",
                        column: x => x.orderID,
                        principalTable: "orders",
                        principalColumn: "order_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authentication_role",
                schema: "userMGT",
                table: "Authentication",
                column: "role");

            migrationBuilder.CreateIndex(
                name: "IX_Calender_date_food_id",
                table: "Calender_date",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_food_id",
                schema: "userMGT",
                table: "Favorites",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_Food_categoryID",
                table: "Food",
                column: "categoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_foods_food_id",
                table: "Meal_foods",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_promotion_promotion_id",
                table: "Meal_promotion",
                column: "promotion_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_reservation_id",
                table: "orders",
                column: "reservation_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_customer_id",
                table: "Reservation",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_staffID",
                table: "Reservation",
                column: "staffID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_tableNo",
                table: "Reservation",
                column: "tableNo");

            migrationBuilder.CreateIndex(
                name: "IX_User",
                schema: "userMGT",
                table: "User",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authentication",
                schema: "userMGT");

            migrationBuilder.DropTable(
                name: "Beverage");

            migrationBuilder.DropTable(
                name: "Calender_date");

            migrationBuilder.DropTable(
                name: "checkout");

            migrationBuilder.DropTable(
                name: "Dessert");

            migrationBuilder.DropTable(
                name: "Favorites",
                schema: "userMGT");

            migrationBuilder.DropTable(
                name: "Food_portions");

            migrationBuilder.DropTable(
                name: "FoodUser");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Main_dish");

            migrationBuilder.DropTable(
                name: "Meal_foods");

            migrationBuilder.DropTable(
                name: "Meal_promotion");

            migrationBuilder.DropTable(
                name: "Side_dish");

            migrationBuilder.DropTable(
                name: "Stater");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "userMGT");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropTable(
                name: "Meal");

            migrationBuilder.DropTable(
                name: "promotion");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "FoodCategory");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "userMGT");

            migrationBuilder.DropTable(
                name: "Staff",
                schema: "userMGT");

            migrationBuilder.DropTable(
                name: "table");

            migrationBuilder.DropTable(
                name: "User",
                schema: "userMGT");
        }
    }
}
