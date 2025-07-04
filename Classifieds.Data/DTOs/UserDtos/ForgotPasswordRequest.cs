﻿using System.ComponentModel.DataAnnotations;

namespace Classifieds.Data.DTOs
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
