﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class PasswordResetViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }
}
