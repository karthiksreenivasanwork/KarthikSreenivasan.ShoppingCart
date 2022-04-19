using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShoppingCart.API.BusinessLogic.MSTests
{
    /// <summary>
    // Summary:
    //     Verifies the logic for each method in the ShoppingCart.API.BusinessLogic.PasswordHashManager.
    /// </summary>
    [TestClass]
    public class PasswordHashManagerTests
    {
        readonly string _passwordFromUser = string.Empty;

        public PasswordHashManagerTests()
        {
            this._passwordFromUser = "Karthik";
        }

        /// <summary>
        /// Verify if the hashed password is in the format - { iterations}.{ salt}.{ hash}
        /// </summary>
        [TestMethod]
        public void Hash_A_Password_And_Verify_Format()
        {
            var passwordHashManager = new PasswordHashManager();

            string hashedPassword = passwordHashManager.Hash(_passwordFromUser);
            var parts = hashedPassword.Split('.', 3);

            //Since the password is 
            Assert.AreEqual(parts.Length, 3);
        }

        // <summary>
        /// Verify the original password against a hashed password.
        /// </summary>
        [TestMethod]
        public void Verify_A_HashedPassword()
        {
            var passwordHashManager = new PasswordHashManager();
            bool passwordVerificationResult = false;
            bool needsUpgrade = false;

            string hashedPassword = passwordHashManager.Hash(_passwordFromUser);
            (passwordVerificationResult, needsUpgrade) = passwordHashManager.Check(hashedPassword, _passwordFromUser);

            Assert.AreEqual(passwordVerificationResult, true);
        }


        // <summary>
        /// Verify the same password generates a different hash.
        /// This is because, NO hashed passwords even with the same password as input must be the same.
        /// </summary>
        [TestMethod]
        public void Verify_Same_Password_Generates_Different_Hash()
        {
            var passwordHashManager = new PasswordHashManager();
            bool passwordVerificationResult = false;
            bool needsUpgrade = false;

            string hashedPasswordOne = passwordHashManager.Hash(_passwordFromUser);
            (passwordVerificationResult, needsUpgrade) = passwordHashManager.Check(hashedPasswordOne, _passwordFromUser);

            Assert.AreEqual(passwordVerificationResult, true);
            Assert.AreEqual(needsUpgrade, false); //This is because we are not changing the iterations using HashingOptions.

            //Reset the variables
            passwordVerificationResult = false;
            needsUpgrade = false;

            string hashedPasswordTwo = passwordHashManager.Hash(_passwordFromUser);
            (passwordVerificationResult, needsUpgrade) = passwordHashManager.Check(hashedPasswordTwo, _passwordFromUser);
            Assert.AreEqual(passwordVerificationResult, true);
            Assert.AreEqual(needsUpgrade, false); //This is because we are not changing the iterations using HashingOptions.

            Assert.AreNotEqual(hashedPasswordOne, hashedPasswordTwo);
        }
    }
}
