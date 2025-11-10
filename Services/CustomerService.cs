using Microsoft.EntityFrameworkCore.Storage;
using MiniCrmApi.Data;
using MiniCrmApi.Models;
using MiniCrmApi.Repositories;

namespace MiniCrmApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly MiniCrmContext context;

        public CustomerService(ICustomerRepository customerRepository, MiniCrmContext context)
        {
            this.customerRepository = customerRepository;
            this.context = context;
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

            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await customerRepository.AddAsync(customer);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task UpdateAsync(int id, Customer customer)
        {
            Customer dbCustomer = await GetByIdAsync(id);

            if (customer == null)
            {
                throw new ArgumentNullException("Customer doesn't null");
            }

            if(!string.IsNullOrEmpty(customer.Name))
            {
                dbCustomer.Name = customer.Name;
            }
            if (!string.IsNullOrEmpty(customer.Address))
            {
                dbCustomer.Address = customer.Address;
            }
            if (!string.IsNullOrEmpty(customer.City))
            {
                dbCustomer.City = customer.City;
            }
            if (!string.IsNullOrEmpty(customer.PhoneNumber))
            {
                dbCustomer.PhoneNumber = customer.PhoneNumber;
            }

            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await customerRepository.UpdateAsync(dbCustomer);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            Customer customer = await GetByIdAsync(id);

            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await customerRepository.DeleteAsync(customer);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
