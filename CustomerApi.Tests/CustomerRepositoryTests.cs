using CustomerApi.Data;
using CustomerApi.Models;
using CustomerApi.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;

namespace CustomerApi.Tests;

public class CustomerRepositoryTests
{
    private readonly CustomerRepository _repository;
    private readonly Mock<ICustomerContext> _mockContext;

    public CustomerRepositoryTests()
    {
        _mockContext = new Mock<ICustomerContext>();
        var logger = new Mock<ILogger<CustomerRepository>>().Object;
        _repository = new CustomerRepository(_mockContext.Object, logger);
    }

    [Fact]
    public async Task GetCustomersAsync_ShouldReturnAllCustomers()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" },
            new Customer { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", PhoneNumber = "5555555555" }
        }.AsQueryable();

        _mockContext.Setup(m => m.Customers).Returns(GetMockSet(customers).Object);

        // Act
        var result = await _repository.GetCustomersAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCustomerAsync_ShouldReturnCustomer_WhenCustomerExists()
    {
        // Arrange
        var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" };
        _mockContext.Setup(m => m.Customers.FindAsync(1)).ReturnsAsync(customer);

        // Act
        var result = await _repository.GetCustomerAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("John", result.FirstName);
    }

    [Fact]
    public async Task GetCustomerAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        _mockContext.Setup(m => m.Customers.FindAsync(1)).ReturnsAsync((Customer)null);

        // Act
        var result = await _repository.GetCustomerAsync(1);

        // Assert
        Assert.Null(result);
    }
//  [Fact]
//     public async Task GetCustomerByEmailAsync_ShouldReturnCustomer_WhenEmailExists()
//     {
//         // Arrange
//          var customers = new List<Customer>
//         {
//             new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" },
//             new Customer { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", PhoneNumber = "5555555555" }
//         }.AsQueryable();

//         _mockContext.Setup(m => m.Customers).Returns(GetMockSet(customers).Object);
//            //.Returns(new AsyncEnumerator<Customer>(customers.GetEnumerator()));

//         // Act
//         var result = await _repository.GetCustomerByEmailAsync("john.doe@example.com");

//         // Assert
//         Assert.NotNull(result);
//         Assert.Equal("john.doe@example.com", result.Email);
//     }


//     [Fact]
//     public async Task GetCustomerByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
//     {
//         // Arrange
//         var customers = new List<Customer>
//         {}.AsQueryable();
//         _mockContext.Setup(m => m.Customers).Returns(GetMockSet(customers).Object);

//         // Act
//         var result = await _repository.GetCustomerByEmailAsync("nonexistent@example.com");

//         // Assert
//         Assert.Null(result);
//     }

    [Fact]
    public async Task AddCustomerAsync_ShouldAddCustomerSuccessfully()
    {
        // Arrange
        var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" };
        _mockContext.Setup(m => m.UpdateAsync(customer)).Returns(Task.CompletedTask);

        // Act
        await _repository.CreateCustomerAsync(customer);
        await _repository.UpdateCustomerAsync(customer);

        // Assert
        _mockContext.Verify(m => m.UpdateAsync(customer), Times.Once);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldUpdateCustomerSuccessfully()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" },
            new Customer { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", PhoneNumber = "5555555555" }
        }.AsQueryable();
        
        var customer = new Customer { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", PhoneNumber = "5555555555" };

        _mockContext.Setup(m => m.Customers).Returns(GetMockSet(customers).Object);
        _mockContext.Setup(m => m.UpdateAsync(customer)).Returns(Task.CompletedTask);

        // Act
        await _repository.UpdateCustomerAsync(customer);

        // Assert
        _mockContext.Verify(m => m.UpdateAsync(customer), Times.Once);
    }

    private Mock<DbSet<T>> GetMockSet<T>(IQueryable<T> queryable) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
          .Returns(new AsyncEnumerator<T>(queryable.GetEnumerator()));
        return mockSet;
    }
}