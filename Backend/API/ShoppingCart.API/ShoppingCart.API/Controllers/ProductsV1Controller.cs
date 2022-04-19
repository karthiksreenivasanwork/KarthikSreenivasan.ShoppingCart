﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.API.Controllers
{
    [Route("api/v1/Products")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Products Controller - V1")]
    public class ProductsV1Controller : ControllerBase
    {
        // GET: api/<ProductsController>
        [HttpGet("JWTAuthorizedMethod")]
        [Authorize]
        public string GetAuthorize()
        {
            bool x = User.Identity.IsAuthenticated;
            var identity = User.Identity as ClaimsIdentity;
            return "value1 from version controller 1" ;
        }
    }
}
