using CustomerApi.Controllers;
using CustomerApi.Data;
using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Tests
{
    /// <summary>
    /// Unit tests for the CustomersController.
    /// </summary>
    public class CustomerControllerTests
    {
        private readonly DbContextOptions<CustomerContext> _dbContextOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerControllerTests"/> class.
        /// </summary>
        public CustomerControllerTests()
        {
            // Configure the in-memory database for testing
            _dbContextOptions = new DbContextOptionsBuilder<CustomerContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        /// <summary>
        /// Tests the GetCustomers method to ensure it returns all customers.
        /// </summary>
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

        /// <summary>
        /// Tests the GetCustomer method to ensure it returns the correct customer by ID.
        /// </summary>
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

        /// <summary>
        /// Tests the GetCustomerByEmail method to ensure it returns the correct customer by email address.
        /// </summary>
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

        /// <summary>
        /// Tests the PostCustomer method to ensure it returns a bad request for an invalid model.
        /// </summary>
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

        /// <summary>
        /// Tests the PostCustomer method to ensure it returns the created customer for a valid model.
        /// </summary>
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

        /// <summary>
        /// Tests the PutCustomer method to ensure it returns a bad request for mismatched IDs.
        /// </summary>
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

        /// <summary>
        /// Tests the PutCustomer method to ensure it returns no content for a valid model.
        /// </summary>
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

        /// <summary>
        /// Tests the PutCustomer method to ensure it returns a bad request for an invalid model.
        /// </summary>
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

        /// <summary>
        /// Tests the DeleteCustomer method to ensure it removes the customer.
        /// </summary>
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

        /// <summary>
        /// Tests the DeleteCustomer method to ensure it returns not found when the customer does not exist.
        /// </summary>
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