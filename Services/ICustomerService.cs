using MiniCrmApi.Models;

namespace MiniCrmApi.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);   
        Task AddAsync(Customer customer);
        Task UpdateAsync(int id, Customer customer);
        Task DeleteAsync(int id);
    }
}
