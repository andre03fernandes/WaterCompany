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
    }
}