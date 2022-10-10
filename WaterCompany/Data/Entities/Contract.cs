﻿using System;
using System.ComponentModel.DataAnnotations;

namespace WaterCompany.Data.Entities
{
    public class Contract : IEntity
    {
        public int Id { get; set; }

        public Client Client { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}(-\d{3})?$", ErrorMessage = "Invalid Postal Code!")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Contract Date")]
        public DateTime ContractDate { get; set; }

        //[Required]
        //public string ContractType { get; set; }

        //[Required]
        //public string PaymentType { get; set; }
    }
}