using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShoppingCart.API.BusinessLogic;
using ShoppingCart.API.DataProvider;
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
        ILogger _logger;

        PasswordHashManager _passwordHashManager;
        User _user;
        IUserDataProvider _userDataProvider;

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="configuration">Dependency injected parameter to get application configuration</param>
        /// <param name="logger">Dependency injected parameter to get logging reference</param>
        public UsersV1Controller(IConfiguration configuration, ILogger<UsersV1Controller> logger)
        {
            this._passwordHashManager = new PasswordHashManager();

            this._user = new User(Coordinator.ProviderType.SQL, configuration);
            this._userDataProvider = this._user.getUserProvider();

            this._logger = logger;
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
                    this._logger.LogInformation($"'{username}' is not a registered user.");
                    successResponseMessage = string.Format("'{0}' is not a registered user.", username);
                    return NotFound(successResponseMessage); //Other ActionResult types - Ok, Exception, Unauthorized, BadRequest, Conflict and Redirect
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Something went wrong. Unable to find the user `{username}`.";
                this._logger.LogError($"{errorMessage} Exception Details: \n {ex}");
                return Problem(detail: errorMessage, statusCode: 500); //Default status code is 500 by default.
            }
            return Ok(successResponseMessage);
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="userModelRegistrationData">New user details</param>
        /// <returns>Returns new user registration success message or error status code 500 with a custom error message otherwise.</returns>
        [HttpPost("register")]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(UserResultModel))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Username {username} already exists")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to add a new user.")]
        public IActionResult PostUser([FromBody] UserModel userModelRegistrationData)
        {
            UserResultModel userModelAfterRegistration = new UserResultModel();

            try
            {
                UserModel userModelToRegister = new UserModel()
                {
                    Username = userModelRegistrationData.Username,
                    Password = _passwordHashManager.Hash(userModelRegistrationData.Password),
                    Email = userModelRegistrationData.Email,
                    Phone = userModelRegistrationData.Phone
                };

                userModelAfterRegistration.UserID = _userDataProvider.addNewUser(userModelToRegister);
                userModelAfterRegistration.Username = userModelRegistrationData.Username;
                userModelAfterRegistration.Email = userModelRegistrationData.Email;
                userModelAfterRegistration.Phone = userModelRegistrationData.Phone;
            }
            catch (UserExistsException uex)
            {
                string errorMessage = $"{uex.Message} {userModelRegistrationData.Username}.";
                this._logger.LogError(errorMessage);
                return Conflict(errorMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = "Something went wrong. Unable to add a new user.";
                this._logger.LogError($"{errorMessage} Exception Details: \n {ex}");
                return Problem(detail: $"{errorMessage}.");
            }

            //CreatedAtAction returns status code 201 response.
            return CreatedAtAction("PostUser", new { id = userModelAfterRegistration.UserID }, userModelAfterRegistration);
        }

        /// <summary>
        /// Returns a JWT token on successful authentication and 401 Unauthorized error response otherwise.
        /// </summary>
        /// <param name="loginModelDataFromUser">Authentication credentials from the user to validate</param>
        /// <returns>Returns JWT token on successful result and </returns>
        [HttpPost("login")]
        [SwaggerResponse(StatusCodes.Status201Created, "Returns a valid 'JSON Web Token' (JWT) token which expires in 30 minutes.", Type = typeof(LoginResultModel))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid User")]
        public IActionResult PostLogin([FromBody] LoginModel loginModelDataFromUser)
        {
            var loginResult = new LoginResultModel();

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
                        loginResult.JwtToken = JwtTokenManager.GenerateToken(loginModelDataFromUser.Username);
                        return CreatedAtAction("PostLogin", loginResult);
                    }
                }
                return Unauthorized("Invalid User");
            }
            catch (Exception ex)
            {
                string errorMessage = "Something went wrong. Unable to login.";
                this._logger.LogError($"{errorMessage} Exception Details: \n {ex}");
                return Problem(detail: $"{errorMessage}");
            }
        }

        #endregion
    }
}
