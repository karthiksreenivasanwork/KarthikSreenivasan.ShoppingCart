using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.API.BusinessLogic
{
    /// <summary>
    /// Summary:
    ///     Exposes the hashing and verification methods to be fulfilled by a specified type.
    /// </summary>
    public interface IPasswordHashManager
    {
        /// <summary>
        /// Hashes a password in the format "{iterations}.{salt}.{hash}"
        /// </summary>
        /// <param name="password">password to hash</param>
        /// <returns>Hashed password</returns>
        string Hash(string password);
        /// <summary>
        /// Verify a given Hash
        /// </summary>
        /// <param name="hash">Hashed password</param>
        /// <param name="password">Original password</param>
        /// <returns>Returns whether the password needs to be upgraded along with the password verification result</returns>
        (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
    }
}
