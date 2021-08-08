using System.ComponentModel.DataAnnotations;

namespace invMed.Data
{
    public class ChangePasswordInput
    {
        [Required(ErrorMessage = "Pole Aktualne hasło jest wymagane.")]
        [StringLength(36, ErrorMessage = "{0} musi mieć minimum {2} znaków i maksymalnie {1} znaków.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessage = "Hasło musi zawierać co najmniej jedną małą literę, wielką literę, znak specjalny oraz cyfrę.")]
        [DataType(DataType.Password)]
        [Display(Name = "Aktualne hasło")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Pole Nowe hasło jest wymagane.")]
        [StringLength(36, ErrorMessage = "{0} musi mieć minimum {2} znaków i maksymalnie {1} znaków.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessage = "Hasło musi zawierać co najmniej jedną małą literę, wielką literę, znak specjalny oraz cyfrę.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("NewPassword", ErrorMessage = "Hasła nie są zgodne.")]
        public string ConfirmNewPassword { get; set; }
    }
}
