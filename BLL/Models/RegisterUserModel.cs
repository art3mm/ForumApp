using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.Models
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "FirstName is incorrect")]
        [RegularExpression("^[a-zA-Z]*$")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is incorrect")]
        [RegularExpression("^[a-zA-Z]*$")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is incorrect")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is incorrect")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation of Password is incorrect")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords should be equal")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "NickName is incorrect")]
        [RegularExpression("^[a-zA-Z0-9]*$")]
        public string NickName { get; set; }
    }
}
