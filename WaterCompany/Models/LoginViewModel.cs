using System.ComponentModel.DataAnnotations;

namespace WaterCompany.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}