using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers
{
    /// <summary>
    /// Represents a controller for managing customer data.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerRepository repository, ILogger<CustomersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public CustomersController(ICustomerRepository @object)
        {
        }

        /// <summary>
        /// Retrieves the list of customers.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of customers.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            _logger.LogInformation("Getting all customers");
            var customers = await _repository.GetCustomersAsync();
            _logger.LogInformation($"Retrieved {customers?.Count()} customers" );

            return Ok(customers);
        }

        /// <summary>
        /// Retrieves a customer by their ID.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the customer if found, or a <see cref="NotFoundResult"/> if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            _logger.LogInformation("Getting customer with ID {Id}", id);
            var customer = await _repository.GetCustomerAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Customer with ID {Id} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Retrieved customer with ID {Id}", id);
            return Ok(customer);
        }

        /// <summary>
        /// Retrieves a customer by their email address.
        /// </summary>
        /// <param name="email">The email address of the customer.</param>
        /// <returns>An <see cref="ActionResult{T}"/> representing the HTTP response containing the customer.</returns>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<Customer>> GetCustomerByEmail(string email)
        {
            _logger.LogInformation("Getting customer with email {Email}", email);
            var customer = await _repository.GetCustomerByEmailAsync(email);
            if (customer == null)
            {
                _logger.LogWarning("Customer with email {Email} not found", email);
                return NotFound();
            }
            _logger.LogInformation("Retrieved customer with email {Email}", email);
            return Ok(customer);
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customer">The customer object to be created.</param>
        /// <returns>An <see cref="ActionResult"/> representing the HTTP response.</returns>
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _logger.LogInformation("Creating a new customer");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for customer creation");
                return BadRequest(ModelState);
            }

            await _repository.CreateCustomerAsync(customer);
            _logger.LogInformation("Created new customer with ID {Id}", customer.Id);
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        /// <summary>
        /// Creates multiple customers in the database.
        /// </summary>
        /// <param name="customers">The list of customers to create.</param>
        /// <returns>An action result containing the created customers.</returns>
        [HttpPost("batch")]
        public async Task<ActionResult<List<Customer>>> PostCustomers(List<Customer> customers)
        {
            _logger.LogInformation("Creating multiple new customers");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for batch customer creation");
                return BadRequest(ModelState);
            }

            await _repository.CreateCustomersAsync(customers);
            _logger.LogInformation("Created {Count} new customers", customers.Count);
            return CreatedAtAction("GetCustomers", customers);
        }

        /// <summary>
        /// Updates a customer with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="customer">The updated customer object.</param>
        /// <returns>An IActionResult representing the result of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            _logger.LogInformation("Updating customer with ID {Id}", id);
            if (id != customer.Id)
            {
                _logger.LogWarning("ID mismatch for customer update: {Id} vs {CustomerId}", id, customer.Id);
                return BadRequest("Id mismatch");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for customer update");
                return BadRequest(ModelState);
            }

            await _repository.UpdateCustomerAsync(customer);
            _logger.LogInformation("Updated customer with ID {Id}", id);
            return NoContent();
        }

        /// <summary>
        /// Deletes a customer with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the deletion.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            _logger.LogInformation("Deleting customer with ID {Id}", id);
            await _repository.DeleteCustomerAsync(id);
            _logger.LogInformation("Deleted customer with ID {Id}", id);
            return NoContent();
        }
    }
}