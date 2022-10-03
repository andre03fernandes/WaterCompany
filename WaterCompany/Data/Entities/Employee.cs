using System;
using System.ComponentModel.DataAnnotations;

namespace WaterCompany.Data.Entities
{
    public class Employee : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string Name => $"{FirstName} {LastName}";

        [MinLength(9, ErrorMessage = "Not a valide phone number!")]
        [MaxLength(9, ErrorMessage = "Not a valide phone number!")]
        public string Telephone { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}(-\d{3})?$", ErrorMessage = "Invalid Postal Code!")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [MinLength(9)]
        [MaxLength(9)]
        [Display(Name = "T.I.N.")]
        public string TIN { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public User User { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://nextlevel25853.blob.core.windows.net/images/noimage.png"
            : $"https://nextlevel25853.blob.core.windows.net/employees/{ImageId}";
    }
}
