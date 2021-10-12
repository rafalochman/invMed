
using System.ComponentModel.DataAnnotations;

namespace invMed.Data.Enums
{
    public enum NotificationTypeEnum
    {
        [Display(Name = "Bliski termin ważności")]
        ExpirationDate,

        [Display(Name = "Mała ilość")]
        SmallAmount
    }
}
