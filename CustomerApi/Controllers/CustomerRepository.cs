using Microsoft.EntityFrameworkCore;
using CustomerApi.Data;
using CustomerApi.Models;
using System.Linq;
using System.Collections.Generic;
namespace CustomerApi.Controllers
{
    /// <summary>
    /// Represents a repository for managing customer data.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ICustomerContext _context;
        private readonly ILogger<CustomerRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="context">The customer context.</param>
        /// <param name="logger">The logger.</param>
        public CustomerRepository(ICustomerContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all customers asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of customers.</returns>
        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            _logger.LogInformation("Retrieving all customers.");
            return await _context.Customers.ToListAsync();
        }

        /// <summary>
        /// Retrieves a customer by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the customer with the specified ID, or null if not found.</returns>
        public async Task<Customer?> GetCustomerAsync(int id)
        {
            _logger.LogInformation("Retrieving customer with ID: {Id}", id);
            return await _context.Customers.FindAsync(id);
        }

        /// <summary>
        /// Retrieves a customer by email asynchronously.
        /// </summary>
        /// <param name="email">The email of the customer to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the customer with the specified email, or null if not found.</returns>
        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            _logger.LogInformation("Retrieving customer with email: {Email}", email);
            if(_context.Customers is null)
            {
                _logger.LogInformation("Customers is null");
                return null;
            }
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        /// <summary>
        /// Creates a new customer asynchronously.
        /// </summary>
        /// <param name="customer">The customer to create.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CreateCustomerAsync(Customer customer)
        {
            _logger.LogInformation("Creating a new customer.");
            await _context.InsertAsync(customer);
        }

        /// <summary>
        /// Creates multiple customers asynchronously.
        /// </summary>
        /// <param name="customers">The customers to create.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CreateCustomersAsync(List<Customer> customers)
        {
            _logger.LogInformation("Creating multiple customers.");
            await _context.InsertAsync(customers);
        }

        /// <summary>
        /// Updates an existing customer asynchronously.
        /// </summary>
        /// <param name="customer">The customer to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateCustomerAsync(Customer customer)
        {
            _logger.LogInformation("Updating customer with ID: {Id}", customer.Id);
            await _context.UpdateAsync(customer);
        }

        /// <summary>
        /// Deletes a customer by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteCustomerAsync(int id)
        {
            _logger.LogInformation("Deleting customer with ID: {Id}", id);
            await _context.DeleteAsync(id);
        }
    }
}