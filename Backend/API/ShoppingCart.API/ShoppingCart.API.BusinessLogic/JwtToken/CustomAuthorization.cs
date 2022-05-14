using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.API.BusinessLogic
{
    /// <summary>
    /// Summary:
    //     Specifies that the method that this attribute is applied to requires role based access if required along with JWT Token authentication.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly Role _role;

        public CustomAuthorize(Role role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            /// ToDo
            /// Custom authorization attribute to provide role based access.
            //Check whether the user is admin or not before validation.
            //Example adding new products is only possible by an admin.
            if(this._role == Role.Admin)
            {

            }

            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return;
            }
        }
    }

    /// <summary>
    /// Filter criteria definition
    /// </summary>
    public enum Role
    {
        User = 0,
        Admin = 1
    }
}
