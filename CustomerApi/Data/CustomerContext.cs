using Microsoft.EntityFrameworkCore;
using CustomerApi.Models;

namespace CustomerApi.Data
{
    /// <summary>
    /// Represents the database context for the Customer API.
    /// </summary>
    public class CustomerContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the DbSet of Customers.
        /// </summary>
        /// <value>A DbSet representing the Customers table in the database.</value>
        public DbSet<Customer> Customers { get; set; }
    }
}