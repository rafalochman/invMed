
using System.ComponentModel.DataAnnotations;

namespace invMed.Data.Enums
{
    public enum ItemTypeEnum
    {
        [Display(Name = "Standardowy")]
        Regular,

        [Display(Name = "Nadwyżka")]
        Over
    }
}
