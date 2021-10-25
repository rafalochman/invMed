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

        public NotificationView(Notification notification)
        {
            Id = notification.Id;
            Type = notification.Type;
            IsNew = notification.IsNew;

            if (notification.Type == NotificationTypeEnum.ExpirationDate)
            {
                ProductName = notification.Item.Product.Name;
                Barcode = notification.Item.BarCode;
                ExpirationDate = notification.Item.ExpirationDate.Value.ToString("dd/MM/yyyy");
            }
            else if (notification.Type == NotificationTypeEnum.SmallAmount)
            {
                ProductName = notification.Product.Name;
                Amount = notification.Product.Amount;
            }
        }
    }
}
