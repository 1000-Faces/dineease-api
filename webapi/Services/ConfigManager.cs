using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Services
{
    public class ConfigManager
    {
        readonly WebApplicationBuilder builder;

        WebApplication? _app;

        public ConfigManager(WebApplicationBuilder builder)
        {
            this.builder = builder;
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            // enabling CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                           builder =>
                           {
                               // adding wildcard to allow any origin
                               builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                           });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // adding the session service
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        public void AddConfiguration(string jsonFilePath)
        {
            // adding the custom json configuration file
            builder.Configuration.AddJsonFile(jsonFilePath, optional: false, reloadOnChange: true);
        }

        public void AddConfiguration(IConfiguration configurations)
        {
            // adding the custom configuration
            builder.Configuration.AddConfiguration(configurations);
        }

        public void ConfigureDBConnection(string key)
        {
            // load connecton string from the appsettings file
            builder.Services.AddDbContext<MainDatabaseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(key))); // "ConnectionStrings:MainDatabase"
        }

        public WebApplication GetApp()
        {
            _app ??= builder.Build();

            return _app;
        }

        public void AllowSwaggerUI()
        {
            if (_app == null) return; // if the app is not built yet, return

            // enabling swagger
            if (_app.Environment.IsDevelopment() || builder.Configuration["AllowSwagger"] == "true")
            {
                _app.UseSwagger();
                _app.UseSwaggerUI();
            }
        }
    }
}
