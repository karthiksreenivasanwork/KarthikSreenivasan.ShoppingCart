using System;
using System.Linq;
using System.Security.Cryptography;

namespace ShoppingCart.API.BusinessLogic
{
    /// <summary>
    /// Summary:
    //     Provides methods for hashing and validating passwords.
    /// </summary>
    public class PasswordHashManager : IPasswordHashManager
    {
        private const int SaltSize = 16; //128 bit.
        private const int KeySize = 32; // 256 bit.

        private static HashingOptions Options
        {
            get
            {
                return new HashingOptions();
            }
        }

        /// <summary>
        /// Hashes a password in the format "{iterations}.{salt}.{hash}"
        /// </summary>
        /// <param name="password">password to hash</param>
        /// <returns>Hashed password</returns>
        public string Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Options.Iterations,
                HashAlgorithmName.SHA256))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{Options.Iterations}.{salt}.{key}";
            }
        }

        /// <summary>
        /// Verify a given Hash
        /// </summary>
        /// <param name="hash">Hashed password</param>
        /// <param name="password">Original password</param>
        /// <returns>
        /// Returns whether the password needs to be upgraded along with the password verification result.
        /// * Verified: Returns true if the password verification was successful and false otherwise.
        /// * NeedsUpgrade: Returns false unless hashing options were changed.
        /// </returns>
        public (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format. " +
                    "Should be formatted as `{ iterations}.{ salt}.{ hash}`");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var needsUpgrade = iterations != Options.Iterations;

            //Verify the password with the hash value and return the results.
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256))
            {
                var keyToCheck = algorithm.GetBytes(KeySize);
                var verified = keyToCheck.SequenceEqual(key);

                return (verified, needsUpgrade);
            }
        }
    }
}
