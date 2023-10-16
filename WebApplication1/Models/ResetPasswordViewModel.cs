﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
		public string Email { get; set; }
	}
}
