using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using CustomerApi.Models;
using CustomerApi.Data;
using System.Linq.Expressions;

namespace CustomerApi.Tests;
public class CustomerContextTests
{
    private readonly Mock<DbSet<Customer>> _mockSet;
    private readonly Mock<CustomerContext> _mockContext;
    private readonly List<Customer> _customerData;

    public CustomerContextTests()
    {
        _mockSet = new Mock<DbSet<Customer>>();
        _mockContext = new Mock<CustomerContext>(new DbContextOptions<CustomerContext>());
        _customerData = new List<Customer>
        {
            new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" },
            new Customer { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", PhoneNumber = "5555555555" }
        };
        
        var queryableData = _customerData.AsQueryable();

        _mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        _mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        _mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        _mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

        _mockContext.Setup(c => c.Customers).Returns(_mockSet.Object);
    }

    [Fact]
    public async Task SelectAsync_ShouldReturnCustomer_WhenCustomerExists()
    {
        // Arrange
        var customerId = 1;
        _mockSet.Setup(m => m.FindAsync(customerId)).ReturnsAsync(_customerData.First(c => c.Id == customerId));

        // Act
        var result = await _mockContext.Object.SelectAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.Id);
    }

    [Fact]
    public async Task SelectAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = 3;
        _mockSet.Setup(m => m.FindAsync(customerId)).ReturnsAsync((Customer)null);

        // Act
        var result = await _mockContext.Object.SelectAsync(customerId);

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customer = new Customer { Id = 3, FirstName = "Nonexistent", LastName = "Customer" ,Email = "new.customer@example.com" ,PhoneNumber = "1234567890"};
        _mockSet.Setup(m => m.FindAsync(customer.Id)).ReturnsAsync((Customer)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _mockContext.Object.UpdateAsync(customer));
    }

    [Fact]
    public async Task InsertAsync_ShouldAddCustomerSuccessfully()
    {
        // Arrange
        var customer = new Customer { Id = 3, FirstName = "New", LastName = "Customer", Email = "new.customer@example.com" ,PhoneNumber = "1234567890"};

        // Act
        await _mockContext.Object.InsertAsync(customer);

        // Assert
        _mockSet.Verify(m => m.AddAsync(customer, It.IsAny<CancellationToken>()), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_ShouldThrowException_WhenCustomerIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _mockContext.Object.InsertAsync((Customer)null));
    }

    [Fact]
    public async Task InsertAsyncList_ShouldAddCustomersSuccessfully()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = 3, FirstName = "New1", LastName = "Customer1", Email = "new1.customer@example.com" ,PhoneNumber = "1234567890"},
            new Customer { Id = 4, FirstName = "New2", LastName = "Customer2", Email = "new2.customer@example.com",PhoneNumber = "1234567890" }
        };

        // Act
        await _mockContext.Object.InsertAsync(customers);

        // Assert
        _mockSet.Verify(m => m.AddRangeAsync(customers, It.IsAny<CancellationToken>()), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsyncList_ShouldThrowException_WhenCustomersIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _mockContext.Object.InsertAsync((List<Customer>)null));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCustomerSuccessfully()
    {
        // Arrange
        var customer = _customerData.First();
        _mockSet.Setup(m => m.FindAsync(customer.Id)).ReturnsAsync(customer);

        // Act
        await _mockContext.Object.DeleteAsync(customer);

        // Assert
        _mockSet.Verify(m => m.Remove(customer), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotDelete_WhenCustomerDoesNotExistOrIsNull()
    {
        // Arrange
        var customer = new Customer { Id = 3, FirstName = "Nonexistent", LastName = "Customer" ,Email = "new2.customer@example.com",PhoneNumber = "1234567890"};
        _mockSet.Setup(m => m.FindAsync(customer.Id)).ReturnsAsync((Customer)null);

        // Act
        await _mockContext.Object.DeleteAsync(customer);

        // Assert
        _mockSet.Verify(m => m.Remove(It.IsAny<Customer>()), Times.Never);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldDeleteCustomerSuccessfully()
    {
        // Arrange
        var customerId = 1;
        var customer = _customerData.First(c => c.Id == customerId);
        _mockSet.Setup(m => m.FindAsync(customerId)).ReturnsAsync(customer);

        // Act
        await _mockContext.Object.DeleteAsync(customerId);

        // Assert
        _mockSet.Verify(m => m.Remove(customer), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldThrowException_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = 3;
        _mockSet.Setup(m => m.FindAsync(customerId)).ReturnsAsync((Customer)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _mockContext.Object.DeleteAsync(customerId));
    }
}