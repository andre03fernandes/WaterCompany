﻿using System.ComponentModel.DataAnnotations;
using System;

namespace WaterCompany.Models
{
    public class DeliveryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime? DeliveryDate { get; set; }
    }
}
