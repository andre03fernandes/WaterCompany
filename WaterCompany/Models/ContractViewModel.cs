using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterCompany.Data.Entities;

namespace WaterCompany.Models
{
    public class ContractViewModel : Contract
    {
        [Display(Name = "Client")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a client.")]
        public int ClientId { get; set; }

        public IEnumerable<SelectListItem> Clients { get; set; }

        //[Display(Name = "Contract Type")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a contract type.")]
        //public int ContractTypeId { get; set; }

        //public IEnumerable<SelectListItem> ContractTypes { get; set; }

        //[Display(Name = "Payment Type")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a payment type.")]
        //public int PaymentTypeId { get; set; }

        //public IEnumerable<SelectListItem> PaymentTypes { get; set; }
    }
}