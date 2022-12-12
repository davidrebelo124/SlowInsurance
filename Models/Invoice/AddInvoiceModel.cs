﻿using SlowInsurance.Models.Vehicle;
using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models.Invoice
{
    public class AddInvoiceModel
    {
        [Required]
        [EnumDataType(typeof(PaymentType), ErrorMessage = "Invalid payment type")]
        public PaymentType PaymentType { get; set; }
        [Required]
        public double Value { get; set; }

        [Required]
        public string? Vehicle { get; set; }
    }
}
