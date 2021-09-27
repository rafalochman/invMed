
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

    public enum InventoryState
    {
        [Display(Name = "Nieaktywna")]
        Inactive,

        [Display(Name = "Aktywna")]
        Active, 

        [Display(Name = "Zakończona")]
        Finished
    }

    public enum SearchType
    {
        [Display(Name = "Produkt")]
        Product,

        [Display(Name = "Egzęplarz")]
        Item
    }

    public enum ExpiredComunicateType
    {
        [Display(Name = "Przeterminowany")]
        Expired,

        [Display(Name = "Bliski")]
        Close
    }
}
