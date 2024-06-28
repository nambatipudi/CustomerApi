using Microsoft.EntityFrameworkCore;
using CustomerApi.Models;


namespace CustomerApi.Data
{
    public class CustomerContext : DbContext, ICustomerContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options) { }

        public virtual DbSet<Customer> Customers { get; set; }

        public ValueTask<Customer?> SelectAsync(int id)
        {
            return Customers.FindAsync(id);
        }

        public Task<Customer?> SelectAsync(string email)
        {
            return Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task UpdateAsync(Customer customer)
        {
            var existingCustomer = await SelectAsync(customer.Id);
            if (existingCustomer == null)
                throw new ArgumentException("Customer does not exist");

            Entry(existingCustomer).CurrentValues.SetValues(customer);
            await SaveChangesAsync();
        }

        public async Task InsertAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            await Customers.AddAsync(customer);
            await SaveChangesAsync();
        }

        public async Task InsertAsync(List<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            await Customers.AddRangeAsync(customers);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var existingCustomer = await SelectAsync(customer.Id);
            if (existingCustomer == null)
                return;

            Customers.Remove(existingCustomer);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await SelectAsync(id);
            if (customer == null)
                throw new ArgumentException("Customer does not exist");

            await DeleteAsync(customer);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
        }
    }
}