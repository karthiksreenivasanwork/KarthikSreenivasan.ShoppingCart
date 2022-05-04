using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingCart.API.BusinessLogic;
using ShoppingCart.API.Models;
using ShoppingCart.API.SQLDataProvider;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.API.Controllers
{

    /// <summary>
    /// Summary:
    ///     Purpose of this controller is to register and perform authentication of a new user.   
    /// </summary>
    [Route("api/v1/users")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "User Controller - V1")]
    public class UsersV1Controller : ControllerBase
    {
        IConfiguration _configuration;
        PasswordHashManager _passwordHashManager;

        /*
         * ToDo - Move this to business logic using a interface to coordinate with the data provider.
         */
        UserDataProvider _userDataProvider;

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="configuration">Dependency injected parameter to get application configuration</param>
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
        public IActionResult GetUser([Required, FromRoute] string username)
        {
            string successResponseMessage = string.Format("'{0}' is a registered user.", username);

            try
            {
                bool userModelEearchResult = _userDataProvider.verifyUserRegistration(username);

                if (!userModelEearchResult)
                {
                    successResponseMessage = string.Format("'{0}' is not a registered user.", username);
                    return NotFound(successResponseMessage); //Other ActionResult types - Ok, Exception, Unauthorized, BadRequest, Conflict and Redirect
                }
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to find the user.", statusCode: 500); //Default status code is 500 by default.
            }
            return Ok(successResponseMessage);
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
        public IActionResult PostUser([FromBody] UserModel userModelRegistrationData)
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
                return Conflict(string.Format(uex.Message, userModelRegistrationData.Username));
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to add a new user.");
            }

            //CreatedAtAction returns status code 201 response.
            return CreatedAtAction("PostUser", string.Format("New user - '{0}' added successfully", userModelRegistrationData.Username));
        }

        /// <summary>
        /// Returns a JWT token on successful authentication and 401 Unauthorized error response otherwise.
        /// </summary>
        /// <param name="loginModelDataFromUser">Authentication credentials from the user to validate</param>
        /// <returns>Returns JWT token on successful result and </returns>
        [HttpPost("login")]
        [SwaggerResponse(StatusCodes.Status201Created, "Returns a valid 'JSON Web Token' (JWT) token which expires in 30 minutes.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid User")]
        public IActionResult PostLogin([FromBody] LoginModel loginModelDataFromUser)
        {
            try
            {
                string hashedPassword = _userDataProvider.returnHashedPassword(loginModelDataFromUser.Username);

                bool passwordVerificationResult = false;
                bool needsUpgrade = false;

                if (!string.IsNullOrEmpty(hashedPassword))
                {
                    (passwordVerificationResult, needsUpgrade) = _passwordHashManager.Check(hashedPassword, loginModelDataFromUser.Password);

                    if (passwordVerificationResult)
                    {
                        return CreatedAtAction("PostLogin", JwtTokenManager.GenerateToken(loginModelDataFromUser.Username));
                    }
                }
                return Unauthorized("Invalid User");
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to login");
            }
        }

        #endregion
    }
}
