using CustomerApi.Models;

namespace CustomerApi.Controllers
{
    /// <summary>
    /// Represents a repository for managing customer data.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Retrieves all customers asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of customers.</returns>
        Task<IEnumerable<Customer>> GetCustomersAsync();

        /// <summary>
        /// Retrieves a customer by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the customer.</returns>
        Task<Customer?> GetCustomerAsync(int id);

        /// <summary>
        /// Retrieves a customer by email asynchronously.
        /// </summary>
        /// <param name="email">The email of the customer to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the customer.</returns>
        Task<Customer?> GetCustomerByEmailAsync(string email);

        /// <summary>
        /// Creates a new customer asynchronously.
        /// </summary>
        /// <param name="customer">The customer to create.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateCustomerAsync(Customer customer);

        /// <summary>
        /// Creates multiple customers asynchronously.
        /// </summary>
        /// <param name="customer">The list of customers to create.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateCustomersAsync(List<Customer> customers);

        /// <summary>
        /// Updates an existing customer asynchronously.
        /// </summary>
        /// <param name="customer">The customer to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateCustomerAsync(Customer customer);

        /// <summary>
        /// Deletes a customer by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteCustomerAsync(int id);
    }

}