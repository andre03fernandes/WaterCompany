using System.ComponentModel.DataAnnotations;

namespace WaterCompany.Data.Entities
{
    public class Offer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Echelon Limit")]
        public string EchelonLimit { get; set; }

        [Display(Name = "Unitary Value")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double UnitaryValue { get; set; }

        public bool IsAvailable { get; set; }

        public User User { get; set; }
    }
}
