using System.ComponentModel.DataAnnotations;

namespace WaterCompany.Data.Entities
{
    public class OrderDetailTemp : IEntity
    {
        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        public Offer Offer { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal UnitaryValue { get; set; }

        public decimal Value => Price * (decimal)UnitaryValue;
    }
}
