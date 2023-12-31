﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using webapi.Models;

#nullable disable

namespace webapi.Migrations
{
    [DbContext(typeof(MainDatabaseContext))]
    partial class MainDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MealPromotion", b =>
                {
                    b.Property<Guid>("MealId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("meal_id");

                    b.Property<Guid>("PromotionId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("promotion_id");

                    b.HasKey("MealId", "PromotionId");

                    b.HasIndex(new[] { "PromotionId" }, "IX_Meal_promotion_promotion_id");

                    b.ToTable("Meal_promotion", (string)null);
                });

            modelBuilder.Entity("webapi.Models.Authentication", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.Property<DateTime>("LastLogged")
                        .HasColumnType("date")
                        .HasColumnName("last_logged");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime")
                        .HasColumnName("last_updated");

                    b.Property<string>("Password")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("password");

                    b.Property<int?>("Role")
                        .HasColumnType("int")
                        .HasColumnName("role");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("salt");

                    b.HasKey("UserId");

                    b.HasIndex(new[] { "Role" }, "IX_Authentication_role");

                    b.ToTable("Authentication");
                });

            modelBuilder.Entity("webapi.Models.Beverage", b =>
                {
                    b.Property<int>("BeverageId")
                        .HasColumnType("int")
                        .HasColumnName("beverage_id");

                    b.Property<int>("FoodId")
                        .HasColumnType("int")
                        .HasColumnName("food_id");

                    b.ToTable("Beverage");
                });

            modelBuilder.Entity("webapi.Models.CalenderDate", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<Guid>("FoodId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("food_id");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.HasKey("Date", "FoodId");

                    b.HasIndex(new[] { "FoodId" }, "IX_Calender_date_food_id");

                    b.ToTable("Calender_date");
                });

            modelBuilder.Entity("webapi.Models.Chat", b =>
                {
                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("MessageID");

                    b.Property<DateTime?>("DateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Message")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<Guid?>("ReceiverId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ReceiverID");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("SenderID");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion")
                        .HasColumnName("timestamp");

                    b.HasKey("MessageId");

                    b.ToTable("chat");
                });

            modelBuilder.Entity("webapi.Models.Checkout", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("orderID");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .IsFixedLength();

                    b.Property<byte[]>("CheckoutTime")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion")
                        .HasColumnName("checkoutTime");

                    b.Property<string>("PaymentMethod")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("paymentMethod")
                        .HasDefaultValueSql("(N'cash')")
                        .IsFixedLength();

                    b.Property<string>("StaffId")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("staffID")
                        .IsFixedLength();

                    b.HasKey("OrderId");

                    b.ToTable("checkout");
                });

            modelBuilder.Entity("webapi.Models.Customer", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.Property<int?>("LoyalityPts")
                        .HasColumnType("int")
                        .HasColumnName("loyality_pts");

                    b.HasKey("UserId")
                        .HasName("PK_Customer_1");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("webapi.Models.Dessert", b =>
                {
                    b.Property<int?>("DessertId")
                        .HasColumnType("int")
                        .HasColumnName("dessert_id");

                    b.Property<int>("FoodId")
                        .HasColumnType("int")
                        .HasColumnName("food_id");

                    b.Property<string>("ServedTemparature")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("served_temparature");

                    b.ToTable("Dessert");
                });

            modelBuilder.Entity("webapi.Models.Favorites", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.Property<int>("FoodId")
                        .HasColumnType("int")
                        .HasColumnName("food_id");

                    b.HasKey("UserId", "FoodId");

                    b.HasIndex(new[] { "FoodId" }, "IX_Favorites_food_id");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("webapi.Models.Food", b =>
                {
                    b.Property<Guid>("FoodId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("food_id");

                    b.Property<string>("Availability")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("availability");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("categoryID");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<string>("FoodImg")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("foodImg");

                    b.Property<string>("FoodName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("food_name");

                    b.Property<string>("FoodType")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("food_type");

                    b.Property<double>("Price")
                        .HasColumnType("float")
                        .HasColumnName("price");

                    b.HasKey("FoodId");

                    b.HasIndex(new[] { "CategoryId" }, "IX_Food_categoryID");

                    b.ToTable("Food");
                });

            modelBuilder.Entity("webapi.Models.FoodCategory", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("categoryID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("categoryName");

                    b.HasKey("CategoryId")
                        .HasName("PK_foodCategory");

                    b.ToTable("FoodCategory");
                });

            modelBuilder.Entity("webapi.Models.FoodPortions", b =>
                {
                    b.Property<int>("FoodId")
                        .HasColumnType("int")
                        .HasColumnName("food_id");

                    b.Property<string>("LargePrice")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("large_price")
                        .IsFixedLength();

                    b.Property<string>("RegularPrice")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("regular_price");

                    b.HasKey("FoodId");

                    b.ToTable("Food_portions");
                });

            modelBuilder.Entity("webapi.Models.FoodUser", b =>
                {
                    b.Property<Guid>("FoodId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CustomerID");

                    b.Property<int?>("OrderCount")
                        .HasColumnType("int");

                    b.HasKey("FoodId", "CustomerId");

                    b.HasIndex(new[] { "CustomerId" }, "IX_FoodUser_CustomerID");

                    b.ToTable("FoodUser");
                });

            modelBuilder.Entity("webapi.Models.Inventory", b =>
                {
                    b.Property<string>("FoodId")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("food_id")
                        .IsFixedLength();

                    b.ToTable("Inventory");
                });

            modelBuilder.Entity("webapi.Models.MainDish", b =>
                {
                    b.Property<int>("FoodId")
                        .HasColumnType("int")
                        .HasColumnName("food_id");

                    b.Property<int>("MainDishId")
                        .HasColumnType("int")
                        .HasColumnName("main_dish_id");

                    b.ToTable("Main_dish");
                });

            modelBuilder.Entity("webapi.Models.Meal", b =>
                {
                    b.Property<Guid>("MealId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("meal_id");

                    b.Property<bool?>("Custom")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("Discription")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("discription");

                    b.Property<string>("MealName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("meal_name");

                    b.Property<double>("Price")
                        .HasColumnType("float")
                        .HasColumnName("price");

                    b.HasKey("MealId");

                    b.ToTable("Meal");
                });

            modelBuilder.Entity("webapi.Models.MealFoods", b =>
                {
                    b.Property<Guid>("MealId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("meal_id");

                    b.Property<Guid>("FoodId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("food_id");

                    b.Property<int?>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("quantity")
                        .HasDefaultValueSql("((1))");

                    b.Property<double?>("TotalPrice")
                        .HasColumnType("float")
                        .HasColumnName("total_price");

                    b.HasKey("MealId", "FoodId")
                        .HasName("PK_Meal_foods_1");

                    b.HasIndex(new[] { "FoodId" }, "IX_Meal_foods_food_id");

                    b.ToTable("Meal_foods");
                });

            modelBuilder.Entity("webapi.Models.OrderFoods", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("order_id");

                    b.Property<Guid>("FoodId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("food_id");

                    b.Property<int?>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("quantity")
                        .HasDefaultValueSql("((1))");

                    b.HasKey("OrderId", "FoodId");

                    b.HasIndex(new[] { "FoodId" }, "IX_Order_Foods_food_id");

                    b.ToTable("Order_Foods");
                });

            modelBuilder.Entity("webapi.Models.OrderMeal", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("order_id");

                    b.Property<Guid>("MealId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("meal_id");

                    b.Property<int?>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("quantity")
                        .HasDefaultValueSql("((1))");

                    b.HasKey("OrderId", "MealId");

                    b.HasIndex("MealId");

                    b.ToTable("Order_Meal");
                });

            modelBuilder.Entity("webapi.Models.Orders", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("order_id");

                    b.Property<double?>("Discount")
                        .HasColumnType("float")
                        .HasColumnName("discount");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nchar(50)")
                        .HasColumnName("order_status")
                        .HasDefaultValueSql("(N'pending')")
                        .IsFixedLength();

                    b.Property<double?>("Price")
                        .HasColumnType("float")
                        .HasColumnName("price");

                    b.Property<Guid?>("PromotionId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("promotionID");

                    b.Property<Guid?>("ReservationId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("reservation_id");

                    b.Property<double>("Total")
                        .HasColumnType("float")
                        .HasColumnName("total");

                    b.HasKey("OrderId");

                    b.HasIndex(new[] { "PromotionId" }, "IX_orders_promotionID");

                    b.HasIndex(new[] { "ReservationId" }, "IX_orders_reservation_id");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("webapi.Models.Promotion", b =>
                {
                    b.Property<Guid>("PromotionId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("promotionID");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("date")
                        .HasColumnName("deadline");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<int?>("Discount")
                        .HasColumnType("int")
                        .HasColumnName("discount");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.HasKey("PromotionId");

                    b.ToTable("promotion");
                });

            modelBuilder.Entity("webapi.Models.Reservation", b =>
                {
                    b.Property<Guid>("ReservationId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("reservation_id");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("customer_id");

                    b.Property<DateTime?>("Departure")
                        .HasColumnType("datetime")
                        .HasColumnName("departure");

                    b.Property<DateTime?>("ReservationDatetime")
                        .HasColumnType("datetime")
                        .HasColumnName("reservation_datetime");

                    b.Property<Guid?>("StaffId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("staff_id");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nchar(50)")
                        .HasColumnName("status")
                        .IsFixedLength();

                    b.Property<int?>("TableNo")
                        .HasColumnType("int")
                        .HasColumnName("tableNo");

                    b.HasKey("ReservationId");

                    b.HasIndex(new[] { "CustomerId" }, "IX_Reservation_customer_id");

                    b.HasIndex(new[] { "StaffId" }, "IX_Reservation_staff_id");

                    b.HasIndex(new[] { "TableNo" }, "IX_Reservation_tableNo");

                    b.ToTable("Reservation");
                });

            modelBuilder.Entity("webapi.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("role_name");

                    b.HasKey("RoleId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("webapi.Models.SideDish", b =>
                {
                    b.Property<int>("FoodId")
                        .HasColumnType("int")
                        .HasColumnName("food_id");

                    b.Property<int>("SideDishId")
                        .HasColumnType("int")
                        .HasColumnName("side_dish_id");

                    b.ToTable("Side_dish");
                });

            modelBuilder.Entity("webapi.Models.Staff", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.Property<byte?>("IsActive")
                        .HasColumnType("tinyint")
                        .HasColumnName("is_active");

                    b.Property<string>("JobTitle")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("job_title");

                    b.HasKey("UserId")
                        .HasName("PK_SupportStaff_1");

                    b.ToTable("Staff");
                });

            modelBuilder.Entity("webapi.Models.Stater", b =>
                {
                    b.Property<int>("FoodId")
                        .HasColumnType("int")
                        .HasColumnName("food_id");

                    b.Property<int>("StaterId")
                        .HasColumnType("int")
                        .HasColumnName("stater_id");

                    b.ToTable("Stater");
                });

            modelBuilder.Entity("webapi.Models.Table", b =>
                {
                    b.Property<int>("TableNo")
                        .HasColumnType("int")
                        .HasColumnName("tableNo");

                    b.Property<string>("Availability")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("availability");

                    b.Property<int?>("Seating")
                        .HasColumnType("int")
                        .HasColumnName("seating");

                    b.HasKey("TableNo");

                    b.ToTable("table");
                });

            modelBuilder.Entity("webapi.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("address");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("date")
                        .HasColumnName("created_date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("name");

                    b.Property<string>("Phone")
                        .HasMaxLength(12)
                        .IsUnicode(false)
                        .HasColumnType("char(12)")
                        .HasColumnName("phone")
                        .IsFixedLength();

                    b.Property<string>("UserImage")
                        .HasColumnType("text")
                        .HasColumnName("user_image");

                    b.Property<string>("UserImageType")
                        .HasMaxLength(6)
                        .IsUnicode(false)
                        .HasColumnType("varchar(6)")
                        .HasColumnName("user_image_type");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .HasColumnName("username")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Email" }, "IX_User")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("MealPromotion", b =>
                {
                    b.HasOne("webapi.Models.Meal", null)
                        .WithMany()
                        .HasForeignKey("MealId")
                        .IsRequired()
                        .HasConstraintName("FK_Meal_promotion_Meal");

                    b.HasOne("webapi.Models.Promotion", null)
                        .WithMany()
                        .HasForeignKey("PromotionId")
                        .IsRequired()
                        .HasConstraintName("FK_Meal_promotion_promotion");
                });

            modelBuilder.Entity("webapi.Models.Authentication", b =>
                {
                    b.HasOne("webapi.Models.Role", "RoleNavigation")
                        .WithMany("Authentication")
                        .HasForeignKey("Role")
                        .HasConstraintName("FK_Authentication_Role");

                    b.HasOne("webapi.Models.User", "User")
                        .WithOne("Authentication")
                        .HasForeignKey("webapi.Models.Authentication", "UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Authentication_User");

                    b.Navigation("RoleNavigation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("webapi.Models.CalenderDate", b =>
                {
                    b.HasOne("webapi.Models.Food", "Food")
                        .WithMany("CalenderDate")
                        .HasForeignKey("FoodId")
                        .IsRequired()
                        .HasConstraintName("FK_Calender_date_Food");

                    b.Navigation("Food");
                });

            modelBuilder.Entity("webapi.Models.Checkout", b =>
                {
                    b.HasOne("webapi.Models.Orders", "Order")
                        .WithOne("Checkout")
                        .HasForeignKey("webapi.Models.Checkout", "OrderId")
                        .IsRequired()
                        .HasConstraintName("FK_checkout_orders1");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("webapi.Models.Customer", b =>
                {
                    b.HasOne("webapi.Models.User", "User")
                        .WithOne("Customer")
                        .HasForeignKey("webapi.Models.Customer", "UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Customer_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("webapi.Models.Food", b =>
                {
                    b.HasOne("webapi.Models.FoodCategory", "Category")
                        .WithMany("Food")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK_Food_FoodCategory");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("webapi.Models.FoodUser", b =>
                {
                    b.HasOne("webapi.Models.Customer", "Customer")
                        .WithMany("FoodUser")
                        .HasForeignKey("CustomerId")
                        .IsRequired()
                        .HasConstraintName("FK_FoodUser_Customer");

                    b.HasOne("webapi.Models.Food", "Food")
                        .WithMany("FoodUser")
                        .HasForeignKey("FoodId")
                        .IsRequired()
                        .HasConstraintName("FK_FoodUser_Food");

                    b.Navigation("Customer");

                    b.Navigation("Food");
                });

            modelBuilder.Entity("webapi.Models.MealFoods", b =>
                {
                    b.HasOne("webapi.Models.Food", "Food")
                        .WithMany("MealFoods")
                        .HasForeignKey("FoodId")
                        .IsRequired()
                        .HasConstraintName("FK_Meal_foods_Food");

                    b.HasOne("webapi.Models.Meal", "Meal")
                        .WithMany("MealFoods")
                        .HasForeignKey("MealId")
                        .IsRequired()
                        .HasConstraintName("FK_Meal_foods_Meal");

                    b.Navigation("Food");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("webapi.Models.OrderFoods", b =>
                {
                    b.HasOne("webapi.Models.Food", "Food")
                        .WithMany("OrderFoods")
                        .HasForeignKey("FoodId")
                        .IsRequired()
                        .HasConstraintName("FK_Order_Foods_Food");

                    b.HasOne("webapi.Models.Orders", "Order")
                        .WithMany("OrderFoods")
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK_Order_Foods_orders");

                    b.Navigation("Food");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("webapi.Models.OrderMeal", b =>
                {
                    b.HasOne("webapi.Models.Meal", "Meal")
                        .WithMany("OrderMeal")
                        .HasForeignKey("MealId")
                        .IsRequired()
                        .HasConstraintName("FK_Order_Meal_Meal");

                    b.HasOne("webapi.Models.Orders", "Order")
                        .WithMany("OrderMeal")
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK_Order_Meal_orders");

                    b.Navigation("Meal");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("webapi.Models.Orders", b =>
                {
                    b.HasOne("webapi.Models.Promotion", "Promotion")
                        .WithMany("Orders")
                        .HasForeignKey("PromotionId")
                        .HasConstraintName("FK_orders_promotion");

                    b.HasOne("webapi.Models.Reservation", "Reservation")
                        .WithMany("Orders")
                        .HasForeignKey("ReservationId")
                        .HasConstraintName("FK_orders_Reservation");

                    b.Navigation("Promotion");

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("webapi.Models.Reservation", b =>
                {
                    b.HasOne("webapi.Models.Customer", "Customer")
                        .WithMany("Reservation")
                        .HasForeignKey("CustomerId")
                        .IsRequired()
                        .HasConstraintName("FK_Reservation_Customer");

                    b.HasOne("webapi.Models.Staff", "Staff")
                        .WithMany("Reservation")
                        .HasForeignKey("StaffId")
                        .HasConstraintName("FK_Reservation_Staff");

                    b.HasOne("webapi.Models.Table", "TableNoNavigation")
                        .WithMany("Reservation")
                        .HasForeignKey("TableNo")
                        .HasConstraintName("FK_Reservation_table");

                    b.Navigation("Customer");

                    b.Navigation("Staff");

                    b.Navigation("TableNoNavigation");
                });

            modelBuilder.Entity("webapi.Models.Customer", b =>
                {
                    b.Navigation("FoodUser");

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("webapi.Models.Food", b =>
                {
                    b.Navigation("CalenderDate");

                    b.Navigation("FoodUser");

                    b.Navigation("MealFoods");

                    b.Navigation("OrderFoods");
                });

            modelBuilder.Entity("webapi.Models.FoodCategory", b =>
                {
                    b.Navigation("Food");
                });

            modelBuilder.Entity("webapi.Models.Meal", b =>
                {
                    b.Navigation("MealFoods");

                    b.Navigation("OrderMeal");
                });

            modelBuilder.Entity("webapi.Models.Orders", b =>
                {
                    b.Navigation("Checkout");

                    b.Navigation("OrderFoods");

                    b.Navigation("OrderMeal");
                });

            modelBuilder.Entity("webapi.Models.Promotion", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("webapi.Models.Reservation", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("webapi.Models.Role", b =>
                {
                    b.Navigation("Authentication");
                });

            modelBuilder.Entity("webapi.Models.Staff", b =>
                {
                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("webapi.Models.Table", b =>
                {
                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("webapi.Models.User", b =>
                {
                    b.Navigation("Authentication");

                    b.Navigation("Customer");
                });
#pragma warning restore 612, 618
        }
    }
}
