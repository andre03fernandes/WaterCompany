using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterCompany.Data.Entities;

namespace WaterCompany.Models
{
    public class ChangeUserViewModel : User
    {
        [Display(Name = "Profile Photo")]
        public IFormFile ImageFile { get; set; }
    }
}