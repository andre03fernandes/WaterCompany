using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using WaterCompany.Data.Entities;

namespace WaterCompany.Models
{
    public class EmployeeViewModel : Employee
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
