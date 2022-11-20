﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterCompany.Data.Entities;

namespace WaterCompany.Models
{
    public class ConsumptionViewModel : Consumption
    {
        [Display(Name = "Client")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a client.")]
        public int ClientId { get; set; }

        public IEnumerable<SelectListItem> Clients { get; set; }
    }
}
