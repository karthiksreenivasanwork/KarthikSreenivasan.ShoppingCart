using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShoppingCart.API.BusinessLogic.MSTests
{
    /// <summary>
    // Summary:
    //     Verifies the logic for each method in the ShoppingCart.API.BusinessLogic.JwtTokenManager.
    /// </summary>
    [TestClass]
    public class JwtTokenManagerTests
    {
        /// <summary>
        /// Verify JWT token structure.
        /// </summary>
        [TestMethod]
        public void Verify_Each_Jwt_Token_Data_Is_Not_Null()
        {
            string jwtToken = JwtTokenManager.GenerateToken("karthik");
            string[] jwtTokenStructureCollection = jwtToken.Split('.');
            string header = jwtTokenStructureCollection[0];
            string payLoad = jwtTokenStructureCollection[1];
            string signature = jwtTokenStructureCollection[2];

            Assert.IsNotNull(header);
            Assert.IsNotNull(payLoad);
            Assert.IsNotNull(signature);
        }

        /// <summary>
        /// Verify two JWT tokens with the same username.
        /// </summary>
        [TestMethod]
        public void Verify_Each_Jwt_Token_Data_With_the_Previous_Token()
        {
            string[] jwtTokenCollection = new string[]
            {
                JwtTokenManager.GenerateToken("karthik"), //First Jwt Token 
                JwtTokenManager.GenerateToken("karthik") //Second Jwt Token
            };

            string[] firstJwtTokenStructureCollection = jwtTokenCollection[0].Split('.');
            string firstJwtTokenHeader = firstJwtTokenStructureCollection[0];
            string firstJwtTokenPayLoad = firstJwtTokenStructureCollection[1];
            string firstJwtTokenSignature = firstJwtTokenStructureCollection[2];

            string[] secondJwtTokenStructureCollection = jwtTokenCollection[1].Split('.');
            string secondJwtTokenHeader = secondJwtTokenStructureCollection[0];
            string secondJwtTokenPayLoad = secondJwtTokenStructureCollection[1];
            string secondJwtTokenSignature = secondJwtTokenStructureCollection[2];

            /*
             * Header contains the algorithms like HMACSHA256.
             * Hence, they will be the same on each call as we use the same algorithm for
             * both JWT tokens that were generated.
             */
            Assert.AreEqual(firstJwtTokenHeader, secondJwtTokenHeader);
            /*
             * Payload contains information like user credentials.
             * Hence, they will be the same on each call.
             */
            Assert.AreEqual(firstJwtTokenPayLoad, secondJwtTokenPayLoad);
            /*
             * Signatire is a combination of base64 encoded Header, Payload along with the secret key.
             * This provides added security.
             */
            Assert.AreEqual(firstJwtTokenSignature, secondJwtTokenSignature);
        }

        /// <summary>
        /// Verify a valid 'JSON Web Token' (JWT) token. 
        /// </summary>
        [TestMethod]
        public void Validate_Jwt_Token_Data()
        {
            string originalUserName = "karthik";
            string jwtToken = JwtTokenManager.GenerateToken("karthik"); //First Jwt Token
            string usernameFromJWT = JwtTokenManager.ValidateToken(jwtToken);
            Assert.AreEqual(originalUserName, usernameFromJWT);
        }

        /// <summary>
        /// Verify an invalid 'JSON Web Token' (JWT) token. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Validate_Tampered_Jwt_Token_Data()
        {
            string originalUserName = "karthik";
            string originalJwtToken = JwtTokenManager.GenerateToken("karthik"); //First Jwt Token
            string tamperedJwtToken = string.Concat(originalJwtToken, "append tampered information");

            string usernameFromJWT = JwtTokenManager.ValidateToken(tamperedJwtToken);
            Assert.AreEqual(originalUserName, usernameFromJWT);
        }
    }
}