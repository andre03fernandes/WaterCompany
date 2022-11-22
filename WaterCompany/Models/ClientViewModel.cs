namespace WaterCompany.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using WaterCompany.Data.Entities;

    public class ClientViewModel : Client
    {
        [Display(Name = "Profile Photo")]
        public IFormFile ImageFile { get; set; }
    }
}