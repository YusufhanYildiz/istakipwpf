using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public class ReportingService : IReportingService
    {
        public async Task<(bool Success, string Message)> GenerateCustomerHistoryPdfAsync(string filePath, Customer customer, IEnumerable<Job> jobs)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    var document = new Document(PageSize.A4, 50, 50, 25, 25);
                    var writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Font setup
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                    document.Add(new Paragraph("Müşteri İş Geçmişi Raporu", titleFont));
                    document.Add(new Paragraph($"Müşteri: {customer.FullName}", headerFont));
                    document.Add(new Paragraph($"Tarih: {DateTime.Now:dd.MM.yyyy}", normalFont));
                    document.Add(new Chunk("\n"));

                    var table = new PdfPTable(4);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 2, 4, 2, 2 });

                    table.AddCell(new PdfPCell(new Phrase("Tarih", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("İş Başlığı", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Durum", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Bitiş", headerFont)));

                    foreach (var job in jobs)
                    {
                        table.AddCell(new Phrase(job.CreatedDate.ToString("dd.MM.yyyy"), normalFont));
                        table.AddCell(new Phrase(job.JobTitle, normalFont));
                        table.AddCell(new Phrase(job.Status.ToString(), normalFont));
                        table.AddCell(new Phrase(job.EndDate?.ToString("dd.MM.yyyy") ?? "-", normalFont));
                    }

                    document.Add(table);
                    document.Close();
                }
                return (true, "PDF raporu başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                return (false, $"PDF oluşturma hatası: {ex.Message}");
            }
        }
    }
}