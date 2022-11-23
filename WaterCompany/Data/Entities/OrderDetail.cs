namespace WaterCompany.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class OrderDetail : IEntity
    {
        public int Id { get; set; }

        public Offer Offer { get; set; }

        [Display(Name = "Echelon (m³)")]
        public double Echelon { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double UnitaryValue { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double Value => Echelon * UnitaryValue;
    }
}