using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Local.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Favorites",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    food_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => new { x.user_id, x.food_id });
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
                });

            migrationBuilder.CreateTable(
                name: "FoodCategory",
                columns: table => new
                {
                    categoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoryName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_foodCategory", x => x.categoryID);
                });

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
                    meal_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meal_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    discription = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    price = table.Column<double>(type: "float", nullable: false),
                    Custom = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal", x => x.meal_id);
                });

            migrationBuilder.CreateTable(
                name: "promotion",
                columns: table => new
                {
                    promotionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    discount = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deadline = table.Column<DateTime>(type: "date", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion", x => x.promotionID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
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
                name: "Staff",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    job_title = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    is_active = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportStaff_1", x => x.user_id);
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
                    seating = table.Column<int>(type: "int", nullable: true),
                    availability = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_table", x => x.tableNo);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    food_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    food_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    categoryID = table.Column<int>(type: "int", nullable: true),
                    food_type = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    availability = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    price = table.Column<double>(type: "float", nullable: false)
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
                    meal_id = table.Column<int>(type: "int", nullable: false),
                    promotion_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    password = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    salt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    role = table.Column<int>(type: "int", nullable: true),
                    last_logged = table.Column<DateTime>(type: "date", nullable: false),
                    last_updated = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authentication", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Authentication_Role",
                        column: x => x.role,
                        principalTable: "Role",
                        principalColumn: "role_id");
                    table.ForeignKey(
                        name: "FK_Authentication_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    loyality_pts = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer_1", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Customer_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Calender_date",
                columns: table => new
                {
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    food_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calender_date", x => new { x.date, x.food_id });
                    table.ForeignKey(
                        name: "FK_Calender_date_Food",
                        column: x => x.food_id,
                        principalTable: "Food",
                        principalColumn: "food_id");
                });

            migrationBuilder.CreateTable(
                name: "Meal_foods",
                columns: table => new
                {
                    meal_id = table.Column<int>(type: "int", nullable: false),
                    food_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    total_price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal_foods_1", x => new { x.meal_id, x.food_id });
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
                name: "FoodUser",
                columns: table => new
                {
                    FoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodUser", x => new { x.FoodId, x.CustomerID });
                    table.ForeignKey(
                        name: "FK_FoodUser_Customer",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_FoodUser_Food",
                        column: x => x.FoodId,
                        principalTable: "Food",
                        principalColumn: "food_id");
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    reservation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    staff_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    tableNo = table.Column<int>(type: "int", nullable: true),
                    reservation_datetime = table.Column<DateTime>(type: "datetime", nullable: true),
                    departure = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.reservation_id);
                    table.ForeignKey(
                        name: "FK_Reservation_Customer",
                        column: x => x.customer_id,
                        principalTable: "Customer",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_Reservation_Staff",
                        column: x => x.staff_id,
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
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reservation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    total = table.Column<double>(type: "float", nullable: false),
                    discount = table.Column<double>(type: "float", nullable: true),
                    price = table.Column<double>(type: "float", nullable: true),
                    order_status = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    promotionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_orders_Reservation",
                        column: x => x.reservation_id,
                        principalTable: "Reservation",
                        principalColumn: "reservation_id");
                    table.ForeignKey(
                        name: "FK_orders_promotion",
                        column: x => x.promotionID,
                        principalTable: "promotion",
                        principalColumn: "promotionID");
                });

            migrationBuilder.CreateTable(
                name: "checkout",
                columns: table => new
                {
                    orderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    staffID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    paymentMethod = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    Amount = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    checkoutTime = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkout", x => x.orderID);
                    table.ForeignKey(
                        name: "FK_checkout_orders1",
                        column: x => x.orderID,
                        principalTable: "orders",
                        principalColumn: "order_id");
                });

            migrationBuilder.CreateTable(
                name: "Order_Foods",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    food_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Foods", x => new { x.order_id, x.food_id });
                    table.ForeignKey(
                        name: "FK_Order_Foods_Food",
                        column: x => x.food_id,
                        principalTable: "Food",
                        principalColumn: "food_id");
                    table.ForeignKey(
                        name: "FK_Order_Foods_orders",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authentication_role",
                table: "Authentication",
                column: "role");

            migrationBuilder.CreateIndex(
                name: "IX_Calender_date_food_id",
                table: "Calender_date",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_food_id",
                table: "Favorites",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_Food_categoryID",
                table: "Food",
                column: "categoryID");

            migrationBuilder.CreateIndex(
                name: "IX_FoodUser_CustomerID",
                table: "FoodUser",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_foods_food_id",
                table: "Meal_foods",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_promotion_promotion_id",
                table: "Meal_promotion",
                column: "promotion_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Foods_food_id",
                table: "Order_Foods",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_promotionID",
                table: "orders",
                column: "promotionID");

            migrationBuilder.CreateIndex(
                name: "IX_orders_reservation_id",
                table: "orders",
                column: "reservation_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_customer_id",
                table: "Reservation",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_staff_id",
                table: "Reservation",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_tableNo",
                table: "Reservation",
                column: "tableNo");

            migrationBuilder.CreateIndex(
                name: "IX_User",
                table: "User",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authentication");

            migrationBuilder.DropTable(
                name: "Beverage");

            migrationBuilder.DropTable(
                name: "Calender_date");

            migrationBuilder.DropTable(
                name: "checkout");

            migrationBuilder.DropTable(
                name: "Dessert");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Food_portions");

            migrationBuilder.DropTable(
                name: "FoodOrders");

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
                name: "Order_Foods");

            migrationBuilder.DropTable(
                name: "Side_dish");

            migrationBuilder.DropTable(
                name: "Stater");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Meal");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "FoodCategory");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "promotion");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "table");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
