
using System.ComponentModel.DataAnnotations;

namespace invMed.Data.Enums
{
    public enum InventoryStateEnum
    {
        [Display(Name = "Nieaktywna")]
        Inactive,

        [Display(Name = "Aktywna")]
        Active, 

        [Display(Name = "Zakończona")]
        Finished
    }
}
