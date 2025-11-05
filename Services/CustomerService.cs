using MiniCrmApi.Models;
using MiniCrmApi.Repositories;

namespace MiniCrmApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await customerRepository.GetAllAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            Customer customer = await customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                throw new KeyNotFoundException($"No customer found with this ID: {id}");
            }

            return customer;
        }

        public async Task AddAsync(Customer customer)
        {
            if(customer == null)
            {
                throw new ArgumentNullException("Customer doesn't null");
            }
            await customerRepository.AddAsync(customer);
        }

        public async Task UpdateAsync(int id, Customer customer)
        {
            Customer dbCustomer = await GetByIdAsync(id);

            if (customer == null)
            {
                throw new ArgumentNullException("Customer doesn't null");
            }

            if(customer.Name != null)
            {
                dbCustomer.Name = customer.Name;
            }
            if (customer.Address != null)
            {
                dbCustomer.Address = customer.Address;
            }
            if (customer.City != null)
            {
                dbCustomer.City = customer.City;
            }
            if (customer.PhoneNumber != null)
            {
                dbCustomer.PhoneNumber = customer.PhoneNumber;
            }

            await customerRepository.UpdateAsync(dbCustomer);
        }

        public async Task DeleteAsync(int id)
        {
            Customer customer = await GetByIdAsync(id);
            await customerRepository.DeleteAsync(customer);
        }
    }
}
