using Microsoft.EntityFrameworkCore;
using webapi.Models;
using webapi;
using webapi.Endpoints;
using Microsoft.Extensions.Options;

var _corsAllOrigins = "_dineEaseAllOriginsPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddDbContext<MainDatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSQLDatabase")));

// enabling CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(_corsAllOrigins,
        builder => 
        {
            // adding wildcard to allow any origin
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
if (builder.Configuration["AllowSwagger"] == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// add CORS middleware
app.UseCors(_corsAllOrigins);

app.MapControllers();

app.MapUserEndpoints();

app.MapAuthenticationEndpoints();

app.MapOrdersEndpoints();

app.MapReservationEndpoints();

app.Run();
