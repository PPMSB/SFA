using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DotNet
{
    public class AxaptaController : ApiController
    {
        [HttpGet]
        [Route("api/axapta/customers")]
        // for testing http://localhost:52525/api/axapta/customers
        public IHttpActionResult GetCustomers()
        {
            try
            {
                // Create a sample object/data
                var data = new
                {
                    success = true,
                    message = "Test successful",
                    customers = new[]
                    {
                        new { id = 1, name = "Customer 1" },
                        new { id = 2, name = "Customer 2" }
                    }
                };

                // WebAPI will automatically serialize this to JSON
                return Ok(data);

                // Alternatively, if you need to manually serialize:
                // string jsonResult = JsonConvert.SerializeObject(data);
                // return Ok(jsonResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}