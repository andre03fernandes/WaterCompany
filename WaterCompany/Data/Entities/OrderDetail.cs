using System.ComponentModel.DataAnnotations;

namespace WaterCompany.Data.Entities
{
    public class OrderDetail : IEntity
    {
        public int Id { get; set; }

        public Offer Offer { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double UnitaryValue { get; set; }

        public decimal Value => Price * (decimal)UnitaryValue;
    }
}