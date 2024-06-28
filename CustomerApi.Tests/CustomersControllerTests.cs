using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApi.Controllers;
using CustomerApi.Models;
using CustomerApi.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;

namespace CustomerApi.Tests
{
    public class CustomersControllerTests
    {
        private readonly CustomersController _controller;
        private readonly Mock<ICustomerRepository> _mockRepository;

        public CustomersControllerTests()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            var logger = new Mock<ILogger<CustomersController>>();
            _controller = new CustomersController(_mockRepository.Object, logger.Object);
        }

        [Fact]
        public async Task GetCustomers_ShouldReturnOkResult_WithListOfCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890"},
                new Customer { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" , PhoneNumber = "5555555555"}
            };
            _mockRepository.Setup(repo => repo.GetCustomersAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetCustomer_ShouldReturnOkResult_WithCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" , PhoneNumber = "1234567890"};
            _mockRepository.Setup(repo => repo.GetCustomerAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomer(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetCustomer_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetCustomerAsync(1)).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.GetCustomer(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCustomerByEmail_ShouldReturnOkResult_WithCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" ,PhoneNumber = "1234567890"};
            _mockRepository.Setup(repo => repo.GetCustomerByEmailAsync("john.doe@example.com")).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerByEmail("john.doe@example.com");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal("john.doe@example.com", returnValue.Email);
        }

        [Fact]
        public async Task GetCustomerByEmail_ShouldReturnNotFound_WhenEmailDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetCustomerByEmailAsync("nonexistent@example.com")).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.GetCustomerByEmail("nonexistent@example.com");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostCustomer_ShouldReturnCreatedAtActionResult_WithCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" ,PhoneNumber = "1234567890"};

            // Act
            var result = await _controller.PostCustomer(customer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Customer>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("John", returnValue.FirstName);
        }

        [Fact]
        public async Task PostCustomer_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("FirstName", "Required");

            // Act
            var result = await _controller.PostCustomer(new Customer { Email = "invalid@example.com", PhoneNumber = "1234567890",FirstName = "John" ,LastName = "Doe"   });

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task PostCustomers_ShouldReturnCreatedAtActionResult_WithCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" ,PhoneNumber = "1234567890"},
                new Customer { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" ,PhoneNumber = "5555555555"}
            };

            // Act
            var result = await _controller.PostCustomers(customers);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<List<Customer>>(createdAtActionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task PostCustomers_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Customers", "Required");

            // Act
            var result = await _controller.PostCustomers(new List<Customer>());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task PutCustomer_ShouldReturnNoContent_WhenCustomerIsUpdated()
        {
            // Arrange
            var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" ,PhoneNumber = "1234567890"};
            _mockRepository.Setup(repo => repo.UpdateCustomerAsync(customer)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutCustomer(1, customer);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutCustomer_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" ,PhoneNumber = "1234567890" };

            // Act
            var result = await _controller.PutCustomer(2, customer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Id mismatch", badRequestResult.Value);
        }

        [Fact]
        public async Task PutCustomer_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("FirstName", "Required");

            // Act
            var result = await _controller.PutCustomer(1, new Customer { Id = 1, Email = "invalid@example.com",PhoneNumber="1234567890",FirstName = "John",LastName = "Doe"  });

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteCustomer_ShouldReturnNoContent_WhenCustomerIsDeleted()
        {
            // Act
            var result = await _controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}