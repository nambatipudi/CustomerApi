using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerApi.Data;
using CustomerApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Controllers
{
    /// <summary>
    /// API Controller for managing customers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CustomersController(CustomerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <returns>A list of customers.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            // Return the list of customers from the database
            return await _context.Customers.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>The customer with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            // Find the customer by ID
            var customer = await _context.Customers.FindAsync(id);
            // If the customer is not found, return a 404 Not Found response
            if (customer == null)
            {
                return NotFound();
            }
            // Return the customer
            return customer;
        }

        /// <summary>
        /// Retrieves a specific customer by email address.
        /// </summary>
        /// <param name="email">The email address of the customer.</param>
        /// <returns>The customer with the specified email address.</returns>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<Customer>> GetCustomerByEmail(string email)
        {
            // Find the customer by email
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            // If the customer is not found, return a 404 Not Found response
            if (customer == null)
            {
                return NotFound();
            }
            // Return the customer
            return customer;
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customer">The customer to create.</param>
        /// <returns>The created customer.</returns>
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Add the customer to the database
            await _context.Customers.AddAsync(customer);
            // Save changes to the database
            await _context.SaveChangesAsync();
            // Return a 201 Created response with the newly created customer
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="customer">The updated customer details.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            // Check if the ID in the URL matches the ID in the customer object
            if (id != customer.Id)
            {
                return BadRequest();
            }

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mark the customer entity as modified
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the customer still exists
                if (!await _context.Customers.AnyAsync(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            // Return a 204 No Content response
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            // Find the customer by ID
            var customer = await _context.Customers.FindAsync(id);
            // If the customer is not found, return a 404 Not Found response
            if (customer == null)
            {
                return NotFound();
            }

            // Remove the customer from the database
            _context.Customers.Remove(customer);
            // Save changes to the database
            await _context.SaveChangesAsync();
            // Return a 204 No Content response
            return NoContent();
        }
    }
}