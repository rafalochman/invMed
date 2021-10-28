using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class AddNewItemInput
    {
        [Required(ErrorMessage = "Pole Barkod jest wymagane.")]
        [Display(Name = "Barkod")]
        public string Barcode { get; set; }

        [Required(ErrorMessage = "Pole Produkt jest wymagane.")]
        [Display(Name = "Produkt")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Pole Miejsce jest wymagane.")]
        [Display(Name = "Miejsce")]
        public string Place { get; set; }
    }
}