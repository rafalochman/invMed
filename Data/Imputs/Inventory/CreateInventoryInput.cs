using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class CreateInventoryInput
    {
        [Required(ErrorMessage = "Pole Typ jest wymagane.")]
        [Display(Name = "Typ")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Pole Opis jest wymagane.")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Pole Data rozpoczęcia jest wymagane.")]
        [Display(Name = "Data rozpoczęcia")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Data planowanego zakończenia")]
        public DateTime? PlanedEndDate { get; set; }
    }
}
