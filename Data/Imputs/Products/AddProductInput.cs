using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class AddProductInput
    {
        [Required(ErrorMessage = "Pole Nazwa jest wymagane.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole Kategoria jest wymagane.")]
        [Display(Name = "Kategoria")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Pole Producent jest wymagane.")]
        [Display(Name = "Producent")]
        public string Producer { get; set; }

        [Required(ErrorMessage = "Pole Dostawca jest wymagane.")]
        [Display(Name = "Dostawca")]
        public string Supplier { get; set; }

        [Required(ErrorMessage = "Pole Cena jest wymagane.")]
        [Display(Name = "Cena")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Pole Ilość minimalna jest wymagane.")]
        [Display(Name = "Ilość minimalna")]
        public int MinAmount { get; set; }

        [Required(ErrorMessage = "Pole Ilość maksymalna jest wymagane.")]
        [Display(Name = "Ilość maksymalna")]
        public int MaxAmount { get; set; }
    }
}