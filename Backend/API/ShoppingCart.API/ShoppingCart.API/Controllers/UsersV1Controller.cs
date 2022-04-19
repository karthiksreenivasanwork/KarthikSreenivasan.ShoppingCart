using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingCart.API.BusinessLogic;
using ShoppingCart.API.Models;
using ShoppingCart.API.SQLDataProvider;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.API.Controllers
{
    /// <summary>
    /// Purpose of this controller is to register and perform authentication of a new user.
    /// </summary>
    [Route("api/v1/users")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "User Controller - V1")]
    public class UsersV1Controller : ControllerBase
    {
        IConfiguration _configuration;
        UserDataProvider _userDataProvider;
        PasswordHashManager _passwordHashManager;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="configuration">Reference to application configuration properties</param>
        public UsersV1Controller(IConfiguration configuration)
        {
            this._configuration = configuration;
            _userDataProvider = new UserDataProvider(configuration);
            _passwordHashManager = new PasswordHashManager();
        }

        #region CRUD methods

        /// <summary>
        /// Check for a registered user.
        /// </summary>
        /// <param name="username">Username of the registered user.</param>
        /// <returns>Returns UserCheckModel reference if user is found or error status code 500 with a custom error message otherwise.</returns>
        [HttpGet("usernamecheck/{username}")]
        [SwaggerResponse(StatusCodes.Status200OK, "{username} is a registered user")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "{username} is not a registered user.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to find the user.")] //Swagger documentation - Error response details
        public IActionResult Get(string username)
        {
            bool userModelEearchResult = _userDataProvider.verifyUserRegistration(username);
            string responseMessage = string.Format("'{0}' is a registered user.", username);

            try
            {
                if (!userModelEearchResult)
                {
                    responseMessage = string.Format("'{0}' is not a registered user.", username);
                    return NotFound(responseMessage); //Other ActionResult types - Ok, Exception, Unauthorized, BadRequest, Conflict and Redirect
                }
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to find the user.", statusCode: 500); //Default status code is 500 by default.
            }
            return Ok(responseMessage);
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="userModelRegistrationData">New user details</param>
        /// <returns>Returns new user registration success message or error status code 500 with a custom error message otherwise.</returns>
        [HttpPost("register")]
        [SwaggerResponse(StatusCodes.Status201Created, "New user - '{username}' added successfully")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Username {username} already exists")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to add a new user.")]
        public IActionResult Post([FromBody] UserModel userModelRegistrationData)
        {
            try
            {
                UserModel userModelToRegister = new UserModel()
                {
                    Username = userModelRegistrationData.Username,
                    Password = _passwordHashManager.Hash(userModelRegistrationData.Password),
                    Email = userModelRegistrationData.Email,
                    Phone = userModelRegistrationData.Phone
                };
                _userDataProvider.addNewUser(userModelToRegister);
            }
            catch (UserExistsException uex)
            {
                return Conflict(string.Format("Username {0} already exists", userModelRegistrationData.Username));
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to add a new user.");
            }

            //CreatedAtAction returns status code 201 response.
            return CreatedAtAction("Post", string.Format("New user - '{0}' added successfully", userModelRegistrationData.Username));
        }

        /// <summary>
        /// Returns a JWT token on successful authentication and 401 Unauthorized error response otherwise.
        /// </summary>
        /// <param name="loginModelDataFromUser">Authentication credentials from the user to validate</param>
        /// <returns>Returns JWT token on successful result and </returns>
        [HttpPost("login")]
        [SwaggerResponse(StatusCodes.Status201Created, "Returns a valid 'JSON Web Token' (JWT) token which expires in 30 minutes.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid User")]
        public IActionResult Post([FromBody] LoginModel loginModelDataFromUser)
        {
            string hashedPassword = _userDataProvider.returnHashedPassword(loginModelDataFromUser.Username);

            bool passwordVerificationResult = false;
            bool needsUpgrade = false;

            if (!string.IsNullOrEmpty(hashedPassword))
            {
                (passwordVerificationResult, needsUpgrade) = _passwordHashManager.Check(hashedPassword, loginModelDataFromUser.Password);

                if (passwordVerificationResult)
                {
                    return CreatedAtAction("Post", JwtTokenManager.GenerateToken(loginModelDataFromUser.Username));
                }
            }
            return Unauthorized("Invalid User");
        }

        #endregion
    }
}
