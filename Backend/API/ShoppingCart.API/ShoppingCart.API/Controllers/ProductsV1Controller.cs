using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.API.Controllers
{
    [ApiVersion("1")] //Not applied to controller route for supporting Swagger implementation.
    [Route("api/v1/Products")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Products Controller - V1")]
    public class ProductsV1Controller : ControllerBase
    {
        // GET: api/<ProductsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1 from version controller 1" };
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
