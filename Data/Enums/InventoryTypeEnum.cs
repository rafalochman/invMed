
using System.ComponentModel.DataAnnotations;

namespace invMed.Data.Enums
{
    public enum InventoryTypeEnum
    {
        [Display(Name = "Pełna")]
        Full,

        [Display(Name = "Częściowa")]
        Partial
    }
}
