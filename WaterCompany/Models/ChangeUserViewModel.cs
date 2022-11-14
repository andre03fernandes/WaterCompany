using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterCompany.Data.Entities;

namespace WaterCompany.Models
{
    public class ChangeUserViewModel
    {
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(9, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [MinLength(9)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^\d{4}(-\d{3})?$", ErrorMessage = "Invalid Postal Code!")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [MinLength(9)]
        [MaxLength(9)]
        [Display(Name = "T.I.N.")]
        public string TIN { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }

        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country.")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string Address { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://nextlevel25853.blob.core.windows.net/images/noimage.png"
            : $"https://nextlevel25853.blob.core.windows.net/users/{ImageId}";

    }
}