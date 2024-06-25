using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Models
{
    /// <summary>
    /// Represents a customer with personal information.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the customer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the prefix for the customer's name.
        /// </summary>
        /// <example>Mr</example>
        [EnumDataType(typeof(NamePrefix))]
        public NamePrefix? Prefix { get; set; }

        /// <summary>
        /// Gets or sets the suffix for the customer's name.
        /// </summary>
        /// <example>Jr</example>
        [EnumDataType(typeof(NameSuffix))]
        public NameSuffix? Suffix { get; set; }

        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        /// <remarks>This field is required and has a maximum length of 50 characters.</remarks>
        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the middle name of the customer.
        /// </summary>
        /// <remarks>This field has a maximum length of 50 characters.</remarks>
        [MaxLength(50)]
        public string? MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        /// <remarks>This field is required and has a maximum length of 50 characters.</remarks>
        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the customer.
        /// </summary>
        /// <remarks>This field is required and must be a valid email address.</remarks>
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the customer.
        /// </summary>
        /// <remarks>This field is required and must be a valid phone number.</remarks>
        [Phone]
        public required string PhoneNumber { get; set; }
    }
}