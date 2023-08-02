using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace webapi.Services
{
    public class PasswordHasher
    {
        const int iterations = 100000;
        
        public static (string, byte[]) Hash(string password, byte[]? salt = null)
        {
            // Generate a 128-bit salt using a sequence of cryptographically strong random bytes.
            byte[] final_salt = salt ?? RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes;


            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: final_salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterations,
                numBytesRequested: 256 / 8));

            return (hashed, final_salt);
        }

        public static bool Verify(string password, string hash, byte[] salt)
        {
            // generate the hash using the same salt and number of iterations
            string new_hash = PasswordHasher.Hash(password, salt).Item1;

            return new_hash == hash;
        }
    }
}
