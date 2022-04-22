using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.API.SQLDataProvider
{
    /// <summary>
    // Summary:
    //     Represents the error that occurs if the username has already been taken.
    /// </summary>
    public class UserExistsException : Exception
    {
        //private string _Message;
        private string _ErrorInformation;

        //public override string Message
        //{
        //    get { return _Message; }
        //}

        public UserExistsException()
        {
            _ErrorInformation = "X";
        }

        public UserExistsException(string username)
            : base(string.Format("Username {0} already exists", username))
        {
        }

    }
}
