using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.WebAPI.Controllers
{
    /// <summary>
    /// Purpose of this controller is to register and perform authentication of a new user.
    /// </summary>
    [ApiVersion("1")]
    [Route("api/v1/users")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "User Controller - V1")]
    public class UsersV1Controller : ControllerBase
    {
        #region CRUD methods

        static List<UserModel> _userModelCollection = new List<UserModel>();

        /// <summary>
        /// Check for a registered user.
        /// </summary>
        /// <param name="username">Username of the registered user.</param>
        /// <returns>Returns UserCheckModel reference if user is found or error status code 500 with a custom error message otherwise.</returns>
        [HttpGet("usernamecheck/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCheckModel))] //Swagger documentation - Success result details
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UserCheckModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] //Swagger documentation - Error response details
        public IActionResult Get(string username)
        {
            bool userModelEearchResult = this.IsUserRegistered(username);
            UserCheckModel userCheckModel = new UserCheckModel()
            {
                RegisteredUser = userModelEearchResult,
                Message = string.Format("'{0}' is a registered user.", username)
            };

            try
            {
                if (!userModelEearchResult)
                {
                    userCheckModel.Message = string.Format("'{0}' does not a registered user.", username);
                    return NotFound(userCheckModel); //Other ActionResult types - Ok, Exception, Unauthorized, BadRequest, Conflict and Redirect
                }
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to find the user.", statusCode: 500); //Default status code is 500 by default.
            }
            return Ok(userCheckModel);
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="userModelRegistrationData">New user details</param>
        /// <returns>Returns new user registration success message or error status code 500 with a custom error message otherwise.</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] UserModel userModelRegistrationData)
        {
            try
            {
                UserModel userModelToRegister = new UserModel()
                {
                    ID = this.GetUserId(),
                    Username = userModelRegistrationData.Username,
                    Password = userModelRegistrationData.Password,
                    Email = userModelRegistrationData.Email,
                    Phone = userModelRegistrationData.Phone
                };

                _userModelCollection.Add(userModelToRegister);

            }
            catch (Exception ex)
            {
                return Problem(detail: "Unable to add a new user.");
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] LoginModel loginModelDataFromUser)
        {
            UserModel authenticatedUser = this.IsUserAuthenticated(loginModelDataFromUser.Username, loginModelDataFromUser.Password);
            if (authenticatedUser != null)
                return CreatedAtAction("Post", "Header.Payload.Signature");
            return Unauthorized("Invalid User");
        }

        #endregion

        #region private methods
        private int GetUserId()
        {
            return _userModelCollection.Count() + 1;
        }

        private bool IsUserRegistered(string username)
        {
            return _userModelCollection.Exists(
                userToFind => userToFind.Username.ToLower() == username.ToLower());
        }

        private UserModel IsUserAuthenticated(string username, string password)
        {
            return _userModelCollection.Find(
                userToFind => userToFind.Username.ToLower() == username.ToLower() && userToFind.Password == password);
        }

        #endregion
    }
}
