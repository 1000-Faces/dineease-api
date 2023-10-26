using webapi.Endpoints;
using Stripe;
using webapi.Services;

var builder = WebApplication.CreateBuilder(args);

// loading services
ConfigManager configManager = new(builder);
// loading the custom secret file
SecretFile secretFile = new("dbconnections");
configManager.AddConfiguration(secretFile.Load());
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

app.MapControllers();

// -------------------- Endpoints --------------------
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
// -------------------- Endpoints --------------------

app.MapMealEndpoints();

app.MapCheckoutEndpoints();

app.Run();
