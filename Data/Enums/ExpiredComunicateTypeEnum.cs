
using System.ComponentModel.DataAnnotations;

namespace invMed.Data.Enums
{
    public enum ExpiredComunicateTypeEnum
    {
        [Display(Name = "Przeterminowany")]
        Expired,

        [Display(Name = "Bliski")]
        Close
    }
}
