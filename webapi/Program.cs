using webapi.Endpoints;
using webapi.Services;

var builder = WebApplication.CreateBuilder(args);

// loading services
ConfigManager configManager = new(builder);
// loading the custom secret file
SecretFile secretFile = new("dbconnections");
configManager.AddConfiguration(secretFile.Load());
// connecting to the database
configManager.ConfigureDBConnection("ConnectionStrings:MainDatabase");

var app = configManager.GetApp();

// Configure the HTTP request pipeline.
configManager.AllowSwaggerUI();

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
