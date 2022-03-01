using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("customers")]
    public class CustomerController : Controller
    {
        [HttpGet("{id:long}")]
        public async Task<ActionResult> GetCustomerAsync([FromRoute] long id)
        {
            try
            {
                Customer reslt = DBHelper.GetCustomer(id);
                if (reslt == null) 
                    return NotFound();
                else 
                    return StatusCode(StatusCodes.Status200OK, reslt);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateCustomerAsync([FromBody] Customer customer)
        {
            try
            {
                var reslt = DBHelper.CreateCustomer(customer);
                return StatusCode(StatusCodes.Status200OK, new { Id = customer.Id });
            }
            catch
            {
                return Conflict();
            }
        }
    }
}