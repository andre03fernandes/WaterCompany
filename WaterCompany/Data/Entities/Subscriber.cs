using System.ComponentModel.DataAnnotations;

namespace WaterCompany.Data.Entities
{
    public class Subscriber : IEntity
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
