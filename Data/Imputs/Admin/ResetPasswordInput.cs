using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ResetPasswordInput
    {
        [Required(ErrorMessage = "Pole Hasło jest wymagane.")]
        [StringLength(36, ErrorMessage = "{0} musi mieć minimum {2} znaków i maksymalnie {1} znaków.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessage = "Hasło musi zawierać co najmniej jedną małą literę, wielką literę, znak specjalny oraz cyfrę.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole Potwierdź hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są zgodne.")]
        public string ConfirmPassword { get; set; }
    }
}
