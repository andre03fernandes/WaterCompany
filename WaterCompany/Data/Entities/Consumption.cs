using System.ComponentModel.DataAnnotations;
using System;

namespace WaterCompany.Data.Entities
{
    public class Consumption
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Consumption Date")]
        public DateTime ConsumptionDate { get; set; }

        public int Echelon { get; set; }

        [Display(Name = "Unitary Value")]
        public decimal UnitaryValue { get; set; }

        [Display(Name = "TotalConsumption")]
        public decimal TotalConsumption { get; set; }
    }
}
