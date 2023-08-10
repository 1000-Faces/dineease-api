using Microsoft.EntityFrameworkCore;
using webapi.Models;
using webapi;
using webapi.Endpoints;
using Microsoft.Extensions.Options;

var specificOrigins = "_dineEaseSpecificOrigins";

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
    //options.AddPolicy(name: specificOrigins,
    //                  builder =>
    //                  {
    //                      builder.WithOrigins("http://localhost:5173/");
    //                  });

    // enable CORS for localhost
    options.AddDefaultPolicy(builder =>
    {
        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapUserEndpoints();

app.MapAuthenticationEndpoints();

app.MapOrdersEndpoints();

app.MapReservationEndpoints();

app.Run();
