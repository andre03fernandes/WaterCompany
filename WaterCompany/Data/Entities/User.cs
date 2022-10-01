using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System;
using WaterCompany.Migrations;
using Microsoft.AspNetCore.Http;

namespace WaterCompany.Data.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string Address { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }
    }
}