using Microsoft.EntityFrameworkCore;
using CustomerApi.Models;

namespace CustomerApi.Data
{
    public interface ICustomerContext
    {
        DbSet<Customer> Customers { get; set; }

        ValueTask<Customer?> SelectAsync(int id);
        Task<Customer?> SelectAsync(string email);

        Task UpdateAsync(Customer customer);
        Task InsertAsync(Customer customer);
        Task InsertAsync(List<Customer> customers);
        Task DeleteAsync(Customer customer);
        Task DeleteAsync(int id);

    }
}