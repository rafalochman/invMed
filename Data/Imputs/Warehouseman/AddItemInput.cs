using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class AddItemInput
    {
        [Required(ErrorMessage = "Pole Miejsce jest wymagane.")]
        [Display(Name = "Miejsce")]
        public string Place { get; set; }

        [Display(Name = "Data ważności")]
        public DateTime? ExpirationDate { get; set; }

        [Required(ErrorMessage = "Pole Produkt jest wymagane.")]
        [Display(Name = "Produkt")]
        public string ProductName { get; set; }
    }
}