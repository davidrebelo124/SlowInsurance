﻿using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models
{
    public class AccountDetailsViewModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? NIF { get; set; }
        [Required]
        public string? IBAN { get; set; }
        [Required]
        public string? Birthday { get; set; }
        [Required]
        public string? Historic { get; set; }
        [Required]
        [Display(Name = "Driver's License Number")]
        public string? DriverLicense { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? EmailChanged { get; set; }
    }
}
