
using System.ComponentModel.DataAnnotations;

namespace invMed.Data.Enums
{
    public enum InventoryType
    {
        [Display(Name = "Pełna")]
        Full,

        [Display(Name = "Częściowa")]
        Partial
    }
}
