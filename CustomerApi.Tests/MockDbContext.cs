using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Tests;

public class MockDbContext : DbContext
{
    public MockDbContext(DbContextOptions<MockDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
}
