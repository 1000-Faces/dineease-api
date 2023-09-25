using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using webapi.Models;
using webapi;
using webapi.Endpoints;
using Microsoft.Extensions.Options;
using Stripe;
using Microsoft.AspNetCore.Builder;

// var _allowSpecificOrigins = "_dineEaseSpecificOriginsPolicy";

var builder = WebApplication.CreateBuilder(args);

// -------------------- Services --------------------
// enabling CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
               builder =>
               {
                   // adding wildcard to allow any origin
                   builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
               });

    //options.AddPolicy(_allowSpecificOrigins,
    //    builder =>
    //    {
    //        // adding wildcard to allow any origin
    //        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    //    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure db context with the connection string
builder.Services.AddDbContext<MainDatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:MainDatabase"]));

// -------------------- Services --------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
if (builder.Configuration["AllowSwagger"] == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();

// add CORS middleware
app.UseCors(); // this is the default policy

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseAuthorization();

app.MapControllers();

app.MapUserEndpoints();

app.MapAuthenticationEndpoints();

app.MapOrdersEndpoints();

app.MapReservationEndpoints();

app.MapFoodEndpoints();
//var foodEndpoints = new FoodEndpoints(app.Environment);
//foodEndpoints.MapEndpoints(app.MapGet("/"));

app.MapPromotionEndpoints();

app.MapTableEndpoints();

app.MapChatEndpoints();

app.MapStaffEndpoints();

app.MapMealEndpoints();

app.MapCheckoutEndpoints();

app.Run();
