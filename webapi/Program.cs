using webapi.Endpoints;
using webapi.Services;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

// loading services
ConfigManager configManager = new(builder);

// loading the custom secret file
// SecretFile secretFile = new("dbconnections");

// configManager.AddConfiguration(secretFile.Load());

// connecting to the database
configManager.ConfigureDBConnection("MainDatabase");


var app = configManager.GetApp();

// Configure the HTTP request pipeline.
configManager.AllowSwaggerUI();

app.UseHttpsRedirection();

// add CORS middleware
app.UseCors(); // this is for the default policy

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

// -------------------- Endpoints --------------------
app.MapUserEndpoints();

app.MapAuthenticationEndpoints();

app.MapOrdersEndpoints();

app.MapReservationEndpoints();

app.MapFoodEndpoints();

app.MapPromotionEndpoints();

app.MapTableEndpoints();

app.MapChatEndpoints();

app.MapStaffEndpoints();

app.MapMealEndpoints();

app.MapCheckoutEndpoints();

// -------------------- Endpoints --------------------

app.Run();
