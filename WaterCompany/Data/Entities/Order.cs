﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WaterCompany.Data.Entities
{
    public class Order : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Order date")]
        [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd hh:mm:tt}", ApplyFormatInEditMode = false)]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:tt}", ApplyFormatInEditMode = false)]
        public DateTime DeliveryDate { get; set; }

        [Required]
        public User User { get; set; }

        public IEnumerable<OrderDetail> Items { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double UnitaryValue => Items == null ? 0 : Items.Sum(i => i.UnitaryValue);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value => Items == null ? 0 : Items.Sum(i => i.Value);

        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm }", ApplyFormatInEditMode = false)]
        public DateTime? OrderDateLocal => this.OrderDate == null ? null : this.OrderDate.ToLocalTime();
    }
}