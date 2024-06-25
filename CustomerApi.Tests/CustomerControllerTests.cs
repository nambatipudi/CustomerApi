using CustomerApi.Controllers;
using CustomerApi.Data;
using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Tests
{
    public class CustomerControllerTests
    {
        private readonly DbContextOptions<CustomerContext> _dbContextOptions;

        public CustomerControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<CustomerContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetCustomers_ReturnsAllCustomers()
        {
            // Arrange
            using var context = new CustomerContext(_dbContextOptions);
            var controller = new CustomersController(context);
            await context.Customers.AddAsync(new Customer { FirstName = "John", LastName = "Doe", Email = "test@example.com", PhoneNumber = "1234567890" });
            await context.SaveChangesAsync();

            // Act
            var result = await controller.GetCustomers();

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value!);
        }

        [Fact]
        public async Task GetCustomer_ReturnsCustomer()
        {
            // Arrange
            using var context = new CustomerContext(_dbContextOptions);
            var controller = new CustomersController(context);
            var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "test@example.com", PhoneNumber = "1234567890" };
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.GetCustomer(customer.Id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Customer>>(result);
            var returnValue = Assert.IsType<Customer>(actionResult.Value);
            Assert.Equal(customer.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetCustomerByEmail_ReturnsCustomer()
        {
            // Arrange
            using var context = new CustomerContext(_dbContextOptions);
            var controller = new CustomersController(context);
            var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "test@example.com", PhoneNumber = "1234567890" };
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.GetCustomerByEmail(customer.Email);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Customer>>(result);
            var returnValue = Assert.IsType<Customer>(actionResult.Value);
            Assert.Equal(customer.Email, returnValue.Email);
        }

        [Fact]
        public async Task PostCustomer_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            using var context = new CustomerContext(_dbContextOptions);
            var controller = new CustomersController(context);
            var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "test@exam@ple.com", PhoneNumber = "1234567890" }; // Invalid Email

            // Manually validate the model
            var validationContext = new ValidationContext(customer);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(customer, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            // Act
            var result = await controller.PostCustomer(customer);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostCustomer_ValidModel_ReturnsCreatedAtAction()
        {
            // Arrange
            using var context = new CustomerContext(_dbContextOptions);
            var controller = new CustomersController(context);
            var customer = new Customer { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", PhoneNumber = "0987654321" };

            // Act
            var result = await controller.PostCustomer(customer);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Customer>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Customer>(createdAtActionResult.Value);
            Assert.Equal(customer.Email, returnValue.Email);
        }

        [Fact]
        public async Task PutCustomer_MismatchedId_ReturnsBadRequest()
        {
            // Arrange
            using var context = new CustomerContext(_dbContextOptions);
            var controller = new CustomersController(context);
            var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" };
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.PutCustomer(2, customer);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutCustomer_ValidModel_ReturnsNoContent()
        {
            // Arrange
            using var context = new CustomerContext(_dbContextOptions);
            var controller = new CustomersController(context);
            var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" };
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            // Update the customer
            customer.LastName = "Smith";

            // Act
            var result = await controller.PutCustomer(customer.Id, customer);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutCustomer_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            using var context = new CustomerContext(_dbContextOptions);
            var controller = new CustomersController(context);
            var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" };
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            // Update the customer with invalid email
            customer.Email = "invalid-email";

            // Manually validate the model
            var validationContext = new ValidationContext(customer);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(customer, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            // Act
            var result = await controller.PutCustomer(customer.Id, customer);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_RemovesCustomer()
        {
            // Arrange
            using var context = new CustomerContext(_dbContextOptions);
            var controller = new CustomersController(context);
            var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" };
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteCustomer(customer.Id);
            var customerInDb = await context.Customers.FindAsync(customer.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(customerInDb);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            using var context = new CustomerContext(_dbContextOptions);
            var controller = new CustomersController(context);

            // Act
            var result = await controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}