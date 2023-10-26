using System.Security.Cryptography;
using System.Text;

namespace webapi.Services
{
    public class SecretFile
    {
        private readonly string dataFolder;

        const int HASH_LENGTH = 32; // SHA-256 hash size
        const int KEY_LENGTH = 32; // Key size. Use 16 for 128-bit encryption, 24 for 192-bit and 32 for 256-bit encryption

        public string SecretName { get; private set; }
        public string SecretPath => Path.Combine(dataFolder, $"{SecretName}.bin");
        public string LockPath => Path.Combine(dataFolder, $"{SecretName}.lock");
        public string SecretBuilderPath => Path.Combine(dataFolder, $"{SecretName}-secret.json");

        public SecretFile(string secretName)
        {
            dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            SecretName = secretName;
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

        public void EncryptSecret()
        {
            if (File.Exists(SecretBuilderPath))
            {
                // Genereate a new random key
                var key = Aes.Create().Key;

                // Read the build JSON file
                string dataString = File.ReadAllText(SecretBuilderPath);

                // Encrypt the secret using the build JSON file
                string encyptedString = EncryptString(dataString, key);

                // Save the encrypted secret to the secret file
                File.WriteAllText(SecretPath, encyptedString);

                // Save the lock file
                SaveLockFile(dataString, key);
            }
            else
            {
                // Handle the case where the build JSON file is missing or corrupted
                throw new FileNotFoundException("Build JSON file not found.");
            }
        }

        public static string EncryptString(string dataString, byte[] key)
        {
            if (string.IsNullOrWhiteSpace(dataString)) return dataString;  // There is no need to encrypt null/empty or already encrypted text

            // Create a new random vector.
            var iv = Aes.Create().IV;

            string encryptedStr = Convert.ToBase64String(Encrypt(dataString, key, iv));

            // Mearge encrypted string and IV as base64 string
            return Convert.ToBase64String(iv) + ";" + encryptedStr;
        }

        public string DecryptSecret()
        {
            if (File.Exists(SecretPath) && File.Exists(LockPath))
            {
                // get data from the lock file
                byte[] storedHash, storedKey;
                (storedHash, storedKey) = ReadLockFile();

                // Read the secret file
                string dataString = File.ReadAllText(SecretPath);

                // Decrypt the secret
                string decryptedString = DecryptString(dataString, storedKey);

                // validate the integrity of the decrypted secret
                if (ValidateIntegrity(Encoding.UTF8.GetBytes(decryptedString), storedHash))
                {
                    // return the decrypted secret
                    return decryptedString;
                }

                // Handle the case where the decrypted secret is corrupted
                throw new CryptographicException("Secret file is corrupted.");
            }

            // Handle the case where the encrypted file or lock file is missing or corrupted
            throw new FileNotFoundException("Secret file or lock file not found.");
        }

        public static string DecryptString(string dataString, byte[] key)
        {
            if (string.IsNullOrWhiteSpace(dataString)) return dataString;  // There is no need to encrypt null/empty or already encrypted text

            // Parse the vector from the encrypted data.
            byte[] vector = Convert.FromBase64String(dataString.Split(';')[0]);

            // Decrypt and return the plain string
            return Decrypt(Convert.FromBase64String(dataString.Split(';')[1]), key, vector);
        }

        private (byte[], byte[]) ReadLockFile()
        {
            // Read the stored hash, key and iv from the lock file
            byte[] storedHash, storedKey;
            using BinaryReader reader = new(File.OpenRead(LockPath));
            storedHash = reader.ReadBytes(HASH_LENGTH);
            storedKey = reader.ReadBytes(KEY_LENGTH);

            return (storedHash, storedKey);
        }

        private void SaveLockFile(string data, byte[] key)
        {
            // Create the lock file
            byte[] dataStream = Encoding.UTF8.GetBytes(data);

            // Save the key and hash of the config data in the lock file
            byte[] configHash;
            configHash = SHA256.HashData(dataStream);

            using BinaryWriter writer = new(File.Open(LockPath, FileMode.Create));
            writer.Write(configHash);
            writer.Write(key);
        }

        private bool IsBuildJsonMismatch()
        {
            if (File.Exists(SecretBuilderPath) && File.Exists(LockPath))
            {
                // Read the stored hash from the lock file
                byte[] storedHash;
                (storedHash, _) = ReadLockFile();

                return !ValidateIntegrity(Encoding.UTF8.GetBytes(File.ReadAllText(SecretBuilderPath)), storedHash);
            }

            // If the build JSON file doesn't exist, it's not a new build
            return true;
        }

        private static bool ValidateIntegrity(byte[] datastream, byte[] storedHash)
        {
            // Compute the hash of the datastream
            byte[] buildJsonHash = SHA256.HashData(datastream);

            // Compare the current hash with the stored hash
            return buildJsonHash.SequenceEqual(storedHash);
        }

        private static byte[] Encrypt(string plainText, byte[] key, byte[] vector)
        {
            // A new key and IV is automatically created when you create a new instance of one of the managed symmetric cryptographic classes
            // using the parameterless Create() method.
            // https://learn.microsoft.com/en-us/dotnet/standard/security/generating-keys-for-encryption-and-decryption
            using Aes aesAlg = Aes.Create();

            // Create an encryptor to perform the stream transform
            using ICryptoTransform encryptor = aesAlg.CreateEncryptor(key, vector);

            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, encryptor, CryptoStreamMode.Write);
            using StreamWriter streamWriter = new(cryptoStream);

            // Write the plain text to the StreamWriter
            streamWriter.Write(plainText);

            // Close the StreamWriter to flush and write the data to the CryptoStream
            streamWriter.Close();

            return output.ToArray();
        }

        private static string Decrypt(byte[] encriptedBytes, byte[] key, byte[] vector)
        {
            // Create an Aes object
            using Aes aesAlg = Aes.Create();

            // Create a decryptor to perform the stream transform
            using ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, vector);

            // Create the streams used for decryption
            using MemoryStream input = new(encriptedBytes);
            using CryptoStream cryptoStream = new(input, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);

            return streamReader.ReadToEnd();
        }
    }
}
