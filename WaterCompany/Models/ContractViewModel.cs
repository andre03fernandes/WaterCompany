namespace WaterCompany.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using WaterCompany.Data.Entities;

    public class ContractViewModel : Contract
    {
        [Display(Name = "Client")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a client.")]
        public int ClientId { get; set; }

        public IEnumerable<SelectListItem> Clients { get; set; }
    }
}