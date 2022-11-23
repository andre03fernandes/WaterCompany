namespace WaterCompany.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Subscriber : IEntity
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
