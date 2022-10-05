using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System;
using WaterCompany.Migrations;
using Microsoft.AspNetCore.Http;

namespace WaterCompany.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        public int CityId { get; set; }

        public City City { get; set; }
    }
}