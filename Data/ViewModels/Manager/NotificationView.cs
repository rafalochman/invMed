using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class NotificationView
    {
        public int Id { get; set; }
        public bool IsNew { get; set; }
        public NotificationTypeEnum Type { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public string ExpirationDate { get; set; }
    }
}
