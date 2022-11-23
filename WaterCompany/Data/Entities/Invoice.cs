namespace WaterCompany.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System;

    public class Invoice : IEntity
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Total to pay")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double Total { get; set; }

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; }

        [Display(Name = "Consumption")]
        public Consumption Consumption { get; set; }

        public Client Client { get; set; }

        public User User { get; set; }
    }
}