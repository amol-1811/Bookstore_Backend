using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Mandatory")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Mandatory")]
        [Compare("NewPassword", ErrorMessage = "Password and Confirm Password should be same")]
        public string ConfirmPassword { get; set; }
    }
}
