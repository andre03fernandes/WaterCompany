using System.ComponentModel.DataAnnotations;
using System;

namespace WaterCompany.Data.Entities
{
    public class Consumption : IEntity
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Consumption Date")]
        public DateTime ConsumptionDate { get; set; }

        public int Echelon { get; set; }

        [Display(Name = "Unitary Value")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double UnitaryValue { get; set; }

        [Display(Name = "Total Consumption")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double TotalConsumption { get; set; }

        public Client Client { get; set; }
    }
}
