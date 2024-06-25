using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [EnumDataType(typeof(NamePrefix))]
        public NamePrefix? Prefix { get; set; }

        [EnumDataType(typeof(NameSuffix))]
        public NameSuffix? Suffix { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [MaxLength(50)]
        public string? MiddleName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public required string PhoneNumber { get; set; }
    }
}