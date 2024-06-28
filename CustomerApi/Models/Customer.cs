using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Models
{
    /// <summary>
    /// Represents a customer.
    /// </summary>
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the prefix of the customer's name.
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// Gets or sets the suffix of the customer's name.
        /// </summary>
        public string? Suffix { get; set; }

        [Required]
        [MaxLength(50)]
        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        public required string FirstName { get; set; }

        [MaxLength(50)]
        /// <summary>
        /// Gets or sets the middle name of the customer.
        /// </summary>
        public string? MiddleName { get; set; }

        [Required]
        [MaxLength(50)]
        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        /// <summary>
        /// Gets or sets the email address of the customer.
        /// </summary>
        public required string Email { get; set; }

        [Phone]
        /// <summary>
        /// Gets or sets the phone number of the customer.
        /// </summary>
        public required string PhoneNumber { get; set; }
    }
}