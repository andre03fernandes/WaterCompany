using System.ComponentModel.DataAnnotations;

namespace WaterCompany.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
