
using System.ComponentModel.DataAnnotations;

namespace invMed.Data.Enums
{
    public enum ReportItemTypeEnum
    {
        [Display(Name = "Niedobór")]
        Shortage,

        [Display(Name = "Nadwyżka")]
        Over
    }
}
