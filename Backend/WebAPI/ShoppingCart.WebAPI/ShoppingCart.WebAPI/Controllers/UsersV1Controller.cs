using Microsoft.AspNetCore.Mvc;
using ShoppingCart.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // GET api/<UsersController>/5
        [HttpGet("usernamecheck/{username}")]
        public IActionResult Get(string username)
        {
            bool userModelEearchResult = this.IsUserRegistered(username);

            try
            {
                if (userModelEearchResult)
                    return Ok(string.Format("'{0}' is a registered user.", username));
                else
                    return BadRequest(string.Format("'{0}' does not a registered user.", username));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong. Unable to find the user.");
            }
        }

        // POST api/<UsersController>
        [HttpPost("register")]
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
                return BadRequest("Unable to add a new user.");
            }

            return Ok(string.Format("New user - '{0}' added successfully", userModelRegistrationData.Username));
        }

        // POST api/<UsersController>
        [HttpPost("login")]
        public string Post([FromBody] LoginModel loginModelDataFromUser)
        {
            string jwtToken = "Invalid User";
            UserModel authenticatedUser = this.IsUserAuthenticated(loginModelDataFromUser.Username, loginModelDataFromUser.Password);
            if (authenticatedUser != null)
                jwtToken = "New JWT Token Generated";
            return jwtToken;
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
