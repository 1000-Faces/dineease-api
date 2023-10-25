using Microsoft.EntityFrameworkCore;
using NeoSmart.SecureStore;
using System.Text.Json;
using webapi.Models;

namespace webapi.Services
{
    public class ConfigManager
    {
        private readonly WebApplicationBuilder builder;

        const string SECRET_FILE_PATH = @"Data\secrets.bin";
        const string SECRET_JSON_PATH = @"Data\secrets.json";
        const string KEY_FILE_PATH = @"Data\secrets.key";

        public ConfigManager(WebApplicationBuilder builder)
        {
            this.builder = builder;
        }

        public void ConfigureServices()
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
        }

        private void SaveSecretFile(Dictionary<string, string> configs)
        {
            using var secretManager = SecretsManager.CreateStore();

            // Create a new key securely with a CSPRNG:
            secretManager.GenerateKey();
            // or use an existing key file: secretManager.LoadKeyFromFile("path/to/file");
            // or securely derive key from passsword: secretManager.LoadKeyFromPassword("p4ssw0rd!");

            foreach (var item in configs)
            {
                secretManager.Set(item.Key, item.Value);
            }

            // Optionally export the keyfile (even if you created the store with a password)
            secretManager.ExportKey(KEY_FILE_PATH);

            // Then save the store if you've made any changes to it
            secretManager.SaveStore(SECRET_FILE_PATH);
        }

        private Dictionary<string, string> LoadSecretFile()
        {
            Dictionary<string, string> configs = new();

            using var secretManager = SecretsManager.LoadStore(SECRET_FILE_PATH);

            secretManager.LoadKeyFromFile(KEY_FILE_PATH);

            foreach (var k in secretManager.Keys)
            {
                // push the key-value pairs to the dictionary
                configs.Add(k, secretManager.Get(k));
            }

            return configs;
        }

        public Dictionary<string, string>? LoadConfigsFromScretFile()
        {
            // check if the screts json exists
            if (File.Exists(SECRET_JSON_PATH))
            {
                // deleting existing scret files
                // File.Delete(SECRET_FILE_PATH);
                // File.Delete(KEY_FILE_PATH);

                // loading the json file into a dictionary
                Dictionary<string, string>? configs = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(SECRET_JSON_PATH));

                // saving the configs to the secret file
                SaveSecretFile(configs!);

                return configs;
            }
            else
            {
                // check if the secret file exists
                if (File.Exists(SECRET_FILE_PATH))
                {
                    // loading the configs from the secret file
                    return LoadSecretFile();
                }
                else
                {
                    // no configs found
                    return null;
                }
            }
        }

        public void ConfigDBConnection()
        {
            Dictionary<string, string>? configs = LoadConfigsFromScretFile() ?? throw new FileNotFoundException("Couldn't load the secret file!");

            // Configure db context with the connection string
            builder.Services.AddDbContext<MainDatabaseContext>(options =>
                options.UseSqlServer(configs["ConnectionStrings:MainDatabase"]));  // builder.Configuration["ConnectionStrings:MainDatabase"]
        }
    }
}
