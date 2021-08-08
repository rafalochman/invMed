using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class EditAccountInput
    {
        [Required(ErrorMessage = "Pole Id jest wymagane.")]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Pole Imię jest wymagane.")]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Pole Nazwa użytkownika jest wymagane.")]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Pole Email jest wymagane.")]
        [EmailAddress(ErrorMessage = "Pole Email nie zawiera poprawnego adresu email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
