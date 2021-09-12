using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ScanItemInput
    {
        [Required(ErrorMessage = "Pole Bar kod jest wymagane.")]
        [Display(Name = "Bar kod")]
        public string? BarCode { get; set; }
    }
}