﻿using System.ComponentModel.DataAnnotations;
using System;

namespace WaterCompany.Data.Entities
{
    public class Invoice : IEntity
    {
        public int Id { get; set; }

        public Contract Contract { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Total to pay")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Total { get; set; }

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; }

        [Display(Name = "Consumption")]
        public Consumption Consumption { get; set; }

        public Client Client { get; set; }

        public User User { get; set; }
    }
}