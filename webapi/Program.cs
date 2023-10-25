using Microsoft.EntityFrameworkCore;
using webapi.Models;
using webapi;
using webapi.Endpoints;
using Microsoft.Extensions.Options;
using webapi.Services;

var builder = WebApplication.CreateBuilder(args);

// loading services
ConfigManager configManager = new(builder);
configManager.ConfigureServices();
configManager.ConfigDBConnection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Configuration["AllowSwagger"] == "true")
{
    // Forcing swagger to be enabled in production
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();

// add CORS middleware
app.UseCors(); // this is for the default policy

app.UseAuthorization();

app.MapControllers();

// -------------------- Endpoints --------------------
app.MapUserEndpoints();

app.MapAuthenticationEndpoints();

app.MapOrdersEndpoints();

app.MapReservationEndpoints();

app.MapFoodEndpoints();

app.MapPromotionEndpoints();

app.MapMealEndpoints();

app.MapTableEndpoints();

app.MapChatEndpoints();

app.MapStaffEndpoints();
// -------------------- Endpoints --------------------

app.Run();
