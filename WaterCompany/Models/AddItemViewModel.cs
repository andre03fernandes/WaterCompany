using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WaterCompany.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Offer")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a offer.")]
        public int OfferId { get; set; }

        public double Echelon { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "The unitary value must be a positive number.")]
        public double UnitaryValue { get; set; }

        public IEnumerable<SelectListItem> Offers { get; set; }
    }
}
