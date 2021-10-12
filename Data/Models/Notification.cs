using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class Notification
    {
        public int Id { get; set; }
        public bool IsNew { get; set; }
        public DateTime CreationDate { get; set; }
        public NotificationTypeEnum Type { get; set; }
        public virtual Item Item { get; set; }
        public virtual Product Product { get; set; }
    }
}
