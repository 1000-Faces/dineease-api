using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace webapi.Services
{
    public class SecretFile
    {
        private readonly string dataFolder;

        const int HASH_LENGTH = 32; // SHA-256 hash size
        const int KEY_LENGTH = 32; // Key size. Use 16 for 128-bit encryption, 24 for 192-bit and 32 for 256-bit encryption
        const int IV_LENGTH = 16; // IV size. Use 16 for 128-bit encryption, 24 for 192-bit and 32 for 256-bit encryption

        public string SecretName { get; private set; }
        public string SecretPath => Path.Combine(dataFolder, $"{SecretName}.bin");
        public string LockPath => Path.Combine(dataFolder, $"{SecretName}.lock");
        public string SecretBuilderPath => Path.Combine(dataFolder, $"{SecretName}-secret.json");

        public SecretFile(string secretName)
        {
            dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            SecretName = secretName;
        }

        public void Save(IConfiguration config)
        {
            // serialize the configuration object to JSON
            string configData = JsonSerializer.Serialize(config.GetChildren(), new JsonSerializerOptions { WriteIndented = true });

            // encrypt the secret
            EncryptSecret(configData);

            // replace the build JSON file with the new configurations
            File.WriteAllText(SecretBuilderPath, configData);
        }

        public IConfiguration Load()
        {
            // check if it's a new build
            if (IsBuildJsonMismatch())
            {
                // if it's a new build, encrypt the secret
                EncryptSecret();
            }

            // decrypt the secret
            string configData = DecryptSecret();

            // deserialize the JSON to a configuration object
            ConfigurationBuilder configurationBuilder = new();
            configurationBuilder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(configData)));
            IConfiguration config = configurationBuilder.Build();

            return config;
        }

        private (byte[], byte[], byte[]) ReadLockFile()
        {
            // Read the stored hash, key and iv from the lock file
            byte[] storedHash, storedKey, storedIV;
            using BinaryReader reader = new(File.OpenRead(LockPath));
            storedHash = reader.ReadBytes(HASH_LENGTH);
            storedKey = reader.ReadBytes(KEY_LENGTH);
            storedIV = reader.ReadBytes(IV_LENGTH);

            return (storedHash, storedKey, storedIV);
        }

        private bool IsBuildJsonMismatch()
        {
            if (File.Exists(SecretBuilderPath))
            {
                // Compute the hash of the current build JSON file
                byte[] buildJsonHash = SHA256.HashData(File.ReadAllBytes(SecretBuilderPath));

                if (File.Exists(LockPath))
                {
                    // get data from the lock file
                    byte[] storedHash;
                    (storedHash, _, _) = ReadLockFile();

                    // Compare the current hash with the stored hash
                    bool isSameHash = buildJsonHash.SequenceEqual(storedHash);

                    // If the hashes are the same, return false (not a new build)
                    if (isSameHash)
                        return false;
                }

                // If the file doesn't exist or the hashes don't match, it's a new build
                return true;
            }

            // If the build JSON file doesn't exist, it's not a new build
            return false;
        }

        private void EncryptSecret()
        {
            if (File.Exists(SecretBuilderPath))
            {
                // Encrypt the secret using the build JSON file
                EncryptSecret(File.ReadAllText(SecretBuilderPath));
            }
            else
            {
                // Handle the case where the build JSON file is missing or corrupted
                throw new FileNotFoundException("Build JSON file not found.");
            }
        }

        private void EncryptSecret(string configData)
        {
            byte[] dataStream = Encoding.UTF8.GetBytes(configData);

            // A new key and IV is automatically created when you create a new instance of one of the managed symmetric cryptographic classes
            // using the parameterless Create() method.
            // https://learn.microsoft.com/en-us/dotnet/standard/security/generating-keys-for-encryption-and-decryption
            using Aes aesAlg = Aes.Create();

            // Create an encryptor to perform the stream transform
            using ICryptoTransform encryptor = aesAlg.CreateEncryptor();
            using MemoryStream input = new(dataStream);
            using FileStream output = new(SecretPath, FileMode.Create, FileAccess.Write);
            using CryptoStream cryptoStream = new(output, encryptor, CryptoStreamMode.Write);

            input.CopyTo(cryptoStream);

            // Save the key and hash of the config data in the lock file
            byte[] configHash;
            configHash = SHA256.HashData(dataStream);

            using BinaryWriter writer = new(File.Open(LockPath, FileMode.Create));
            writer.Write(configHash);
            writer.Write(aesAlg.Key);
            writer.Write(aesAlg.IV);
        }

        private string DecryptSecret()
        {
            if (File.Exists(SecretPath) && File.Exists(LockPath))
            {
                // get data from the lock file
                byte[] storedKey, storedIV;
                (_, storedKey, storedIV) = ReadLockFile();

                // Create an Aes object
                using Aes aesAlg = Aes.Create();

                // Create a decryptor to perform the stream transform
                using ICryptoTransform decryptor = aesAlg.CreateDecryptor(storedKey, storedIV);

                // Create the streams used for decryption
                using FileStream input = new(SecretPath, FileMode.Open, FileAccess.Read);
                using MemoryStream output = new();
                using CryptoStream csDecrypt = new(output, decryptor, CryptoStreamMode.Write);
                
                input.CopyTo(csDecrypt);

                // Decryption successful, return the decrypted data
                return Encoding.UTF8.GetString(output.ToArray());
            }

            // Handle the case where the encrypted file or lock file is missing or corrupted
            throw new FileNotFoundException("Secret file or lock file not found.");
        }
    }
}
