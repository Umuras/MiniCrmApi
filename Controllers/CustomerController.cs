using Microsoft.AspNetCore.Mvc;
using MiniCrmApi.Models;
using MiniCrmApi.Services;

namespace MiniCrmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        // GET: api/customer
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Customer> customers = await customerService.GetAllAsync();
            return Ok(customers);
        }

        //GET: api/customer/4
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Customer customer = await customerService.GetByIdAsync(id);
            return Ok(customer);
        }


        //POST: api/customer
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //otomatik validasyon kontrolü
            }

            await customerService.AddAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }


        //PUT: api/customer/4
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Customer customer)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await customerService.UpdateAsync(id, customer);

            return NoContent();
        }


        //DELETE: api/delete/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await customerService.DeleteAsync(id);
            return NoContent();
        }
    }
}
