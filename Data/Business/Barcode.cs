using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using BarcodeLib;

namespace invMed.Data.Business
{
    public class Barcode
    {
        public string Value { get; set; }
        public string UrlData { get; set; }

        public Barcode(string barcodeValue)
        {
            Value = barcodeValue;
            UrlData = GenerateUrlData(barcodeValue);
        }

        private string GenerateUrlData(string barcodeValue)
        {
            const int Width = 250;
            const int Height = 100;

            var barcode = new BarcodeLib.Barcode();
            var barcodeImage = barcode.Encode(TYPE.CODE128, barcodeValue, Width, Height);

            var thumbnail = barcodeImage.GetThumbnailImage(Width, Height, () => false, IntPtr.Zero);
            var imageConverter = new ImageConverter();
            var thumbnailBytes = (byte[])imageConverter.ConvertTo(thumbnail, typeof(byte[]));

            var barcodeImageBase64 = Convert.ToBase64String(thumbnailBytes);
            var urlData = string.Format("data:image/jpg;base64, {0}", barcodeImageBase64);

            return urlData;
        }
    }
}
