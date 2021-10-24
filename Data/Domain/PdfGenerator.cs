using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data.Domain
{
    public class PdfGenerator
    {
        public void DownloadPdf(IJSRuntime jsRuntime, ReportView report, string filename)
        {
            jsRuntime.InvokeVoidAsync("DownloadPdf", filename, Convert.ToBase64String(PdfReport(report)));
        }

        private byte[] PdfReport(ReportView report)
        {
            var memoryStream = new MemoryStream();

            var pdf = new Document(PageSize.A4, 70, 70, 50, 50);
            pdf.AddTitle("Raport");
            pdf.AddSubject("Raport z inwentaryzacji");
            pdf.AddCreationDate();

            PdfWriter.GetInstance(pdf, memoryStream);

            pdf.Open();

            var header = new Paragraph("invMed", FontFactory.GetFont("Arial", 15, Font.BOLD));
            header.SpacingAfter = 12;
            pdf.Add(header);

            var title = new Paragraph("Raport z inwentaryzacji", FontFactory.GetFont("Arial", 14, Font.BOLD));
            title.SpacingAfter = 12;
            pdf.Add(title);

            var font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 12);
            var boldFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 12, Font.BOLD);
            var titleBoldFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 13, Font.BOLD);

            var reportName = new Paragraph
            {
                new Chunk("Nazwa: ", boldFont),
                new Chunk(report.Name, font)
            };
            pdf.Add(reportName);

            var reportDescription = new Paragraph
            {
                new Chunk("Opis: ", boldFont),
                new Chunk(report.Description, font)
            };
            pdf.Add(reportDescription);

            var generationDate = new Paragraph
            {
                new Chunk("Data wygenerowania: ", boldFont),
                new Chunk(report.GenerationDate, font)
            };
            pdf.Add(generationDate);

            var shortageNumber = new Paragraph
            {
                new Chunk("Liczba niedoborów: ", boldFont),
                new Chunk(report.ShortageNumber.ToString(), font)
            };
            pdf.Add(shortageNumber);

            var overNumber = new Paragraph
            {
                new Chunk("Liczba nadwyżek: ", boldFont),
                new Chunk(report.OverNumber.ToString(), font)
            };
            overNumber.SpacingAfter = 6;
            pdf.Add(overNumber);

            var inventoryName = new Paragraph
            {
                new Chunk("Nazwa inwentaryzacji: ", boldFont),
                new Chunk(report.InventoryName, font)
            };
            pdf.Add(inventoryName);

            var inventoryDescription = new Paragraph
            {
                new Chunk("Opis inwetaryzacji: ", boldFont),
                new Chunk(report.InventoryDescription, font)
            };
            pdf.Add(inventoryDescription);

            var inventoryType = new Paragraph
            {
                new Chunk("Typ inwetaryzacji: ", boldFont),
                new Chunk(report.InventoryType, font)
            };
            pdf.Add(inventoryType);

            if(report.PlacesNames is not null)
            {
                var placesNames = new Paragraph
                {
                    new Chunk("Miejsca inwetaryzacji: ", boldFont),
                    new Chunk(report.PlacesNames, font)
                };
                pdf.Add(placesNames);
            }

            var inventoryStartDate = new Paragraph
            {
                new Chunk("Data rozpoczęcia inwetaryzacji: ", boldFont),
                new Chunk(report.InventoryStartDate, font)
            };
            pdf.Add(inventoryStartDate);

            var inventoryFinishDate = new Paragraph
            {
                new Chunk("Data zakończenia inwetaryzacji: ", boldFont),
                new Chunk(report.InventoryFinishDate, font)
            };
            pdf.Add(inventoryFinishDate);

            var warehousemenNames = new Paragraph
            {
                new Chunk("Magazynierzy przeprowadzający inwentaryzację: ", boldFont),
                new Chunk(report.WarehousemenNames, font)
            };
            warehousemenNames.SpacingAfter = 12;
            pdf.Add(warehousemenNames);

            var shortages = new Paragraph("Niedobory", titleBoldFont);
            shortages.SpacingAfter = 12;
            pdf.Add(shortages);

            PdfPTable shortagesTable = new PdfPTable(3);
            shortagesTable.WidthPercentage = 100f;
            shortagesTable.SetWidths(new float[] { 2f, 5f, 1f });

            var barcode = new PdfPCell(new Phrase("Bar kod", boldFont));
            barcode.HorizontalAlignment = 1;
            shortagesTable.AddCell(barcode);

            var product = new PdfPCell(new Phrase("Produkt", boldFont));
            product.HorizontalAlignment = 1;
            shortagesTable.AddCell(product);

            var place = new PdfPCell(new Phrase("Miejsce", boldFont));
            place.HorizontalAlignment = 1;
            shortagesTable.AddCell(place);

            foreach (var item in report.ShortageItems)
            {
                shortagesTable.AddCell(new Phrase(item.BarCode, font));
                shortagesTable.AddCell(new Phrase(item.ProductName, font));
                shortagesTable.AddCell(new Phrase(item.Place, font));
            }
            shortagesTable.SpacingAfter = 12;

            pdf.Add(shortagesTable);

            var overs = new Paragraph("Nadwyżki", titleBoldFont);
            overs.SpacingAfter = 12;
            pdf.Add(overs);

            PdfPTable oversTable = new PdfPTable(2);
            oversTable.WidthPercentage = 100f;
            oversTable.SetWidths(new float[] { 2f, 6f });

            oversTable.AddCell(barcode);

            oversTable.AddCell(product);

            foreach (var item in report.OverItems)
            {
                oversTable.AddCell(new Phrase(item.BarCode, font));
                oversTable.AddCell(new Phrase(item.ProductName, font));
            }

            pdf.Add(oversTable);

            pdf.Close();
            return memoryStream.ToArray();
        }
    }
}
