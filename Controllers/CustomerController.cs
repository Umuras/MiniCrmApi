using Microsoft.AspNetCore.Mvc;
using MiniCrmApi.Dtos;
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
            List<Customer> dbCustomers = await customerService.GetAllAsync();
            List<CustomerResponseDto> customerList = dbCustomers.Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                City = c.City,
                PhoneNumber = c.PhoneNumber
            }).ToList();

            return Ok(customerList);
        }

        //GET: api/customer/4
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Customer dbCustomer = await customerService.GetByIdAsync(id);
            CustomerResponseDto customer = new CustomerResponseDto
            {
                Id = dbCustomer.Id,
                Name = dbCustomer.Name,
                Address = dbCustomer.Address,
                City = dbCustomer.City,
                PhoneNumber = dbCustomer.PhoneNumber,
            };

            return Ok(customer);
        }


        //POST: api/customer
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomerRequestDto customerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //otomatik validasyon kontrolü
            }

            Customer customer = new Customer
            {
                Name = customerRequest.Name,
                Address = customerRequest.Address,
                City = customerRequest.City,
                PhoneNumber = customerRequest.PhoneNumber
            };

            await customerService.AddAsync(customer);

            CustomerResponseDto customerResponse = new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address,
                City = customer.City,
                PhoneNumber = customer.PhoneNumber
            };

            return CreatedAtAction(nameof(GetById), new { id = customerResponse.Id }, customerResponse);
        }


        //PUT: api/customer/4
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerRequestDto customerRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer = new Customer
            {
                Name = customerRequest.Name,
                Address = customerRequest.Address,
                City = customerRequest.City,
                PhoneNumber = customerRequest.PhoneNumber
            };

            await customerService.UpdateAsync(id, customer);

            return Ok("Customer updated successfully");
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
