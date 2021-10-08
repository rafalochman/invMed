using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class CreateReportInput
    {
        [Required(ErrorMessage = "Pole Typ jest wymagane.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole Opis jest wymagane.")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Pole Inwentaryzacja jest wymagane.")]
        [Display(Name = "Inwentaryzacja")]
        public InventoryDto InventoryDto { get; set; }
    }
}
