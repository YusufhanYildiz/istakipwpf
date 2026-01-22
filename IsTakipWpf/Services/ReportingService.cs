using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public class ReportingService : IReportingService
    {
        private static BaseFont _baseFontTr;
        private static BaseFont BaseFontTr
        {
            get
            {
                if (_baseFontTr == null)
                {
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "arial.ttf");
                    _baseFontTr = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                return _baseFontTr;
            }
        }

        private string GetEnumDescription(Enum value)
        {
            if (value == null) return string.Empty;
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr != null ? attr.Description : value.ToString();
        }

        public async Task<(bool Success, string Message)> GenerateCustomerHistoryPdfAsync(string filePath, Customer customer, IEnumerable<Job> jobs)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    Font titleFont = new Font(BaseFontTr, 18, Font.BOLD);
                    Font headerFont = new Font(BaseFontTr, 12, Font.BOLD);
                    Font normalFont = new Font(BaseFontTr, 10, Font.NORMAL);

                    document.Add(new Paragraph("Müşteri İş Geçmişi Raporu", titleFont));
                    document.Add(new Paragraph("Müşteri: " + customer.FullName, headerFont));
                    document.Add(new Paragraph("Tarih: " + DateTime.Now.ToString("dd.MM.yyyy"), normalFont));
                    document.Add(new Chunk("\n"));

                    PdfPTable table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 2, 3, 4, 2, 2 });

                    table.AddCell(new PdfPCell(new Phrase("Tarih", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("İş Başlığı", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Açıklama", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Durum", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Bitiş", headerFont)));

                    foreach (Job job in jobs)
                    {
                        table.AddCell(new Phrase(job.CreatedDate.ToString("dd.MM.yyyy"), normalFont));
                        table.AddCell(new Phrase(job.JobTitle, normalFont));
                        table.AddCell(new Phrase(job.Description, normalFont));
                        table.AddCell(new Phrase(GetEnumDescription(job.Status), normalFont));
                        table.AddCell(new Phrase(job.EndDate.HasValue ? job.EndDate.Value.ToString("dd.MM.yyyy") : "-", normalFont));
                    }

                    document.Add(table);
                    document.Close();
                }
                return (true, "PDF raporu başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                return (false, "PDF oluşturma hatası: " + ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> ExportCustomersToPdfAsync(string filePath, IEnumerable<Customer> customers)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    Font titleFont = new Font(BaseFontTr, 18, Font.BOLD);
                    Font headerFont = new Font(BaseFontTr, 12, Font.BOLD);
                    Font normalFont = new Font(BaseFontTr, 10, Font.NORMAL);

                    document.Add(new Paragraph("Müşteri Listesi", titleFont));
                    document.Add(new Paragraph("Tarih: " + DateTime.Now.ToString("dd.MM.yyyy"), normalFont));
                    document.Add(new Chunk("\n"));

                    PdfPTable table = new PdfPTable(4);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 2, 2, 2, 4 });

                    table.AddCell(new PdfPCell(new Phrase("Ad", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Soyad", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Telefon", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Adres", headerFont)));

                    foreach (Customer c in customers)
                    {
                        table.AddCell(new Phrase(c.FirstName, normalFont));
                        table.AddCell(new Phrase(c.LastName, normalFont));
                        table.AddCell(new Phrase(c.PhoneNumber, normalFont));
                        table.AddCell(new Phrase(c.Address, normalFont));
                    }

                    document.Add(table);
                    document.Close();
                }
                return (true, "Müşteri listesi PDF olarak dışa aktarıldı.");
            }
            catch (Exception ex)
            {
                return (false, "PDF oluşturma hatası: " + ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> ExportJobsToPdfAsync(string filePath, IEnumerable<Job> jobs)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    Font titleFont = new Font(BaseFontTr, 18, Font.BOLD);
                    Font headerFont = new Font(BaseFontTr, 12, Font.BOLD);
                    Font normalFont = new Font(BaseFontTr, 10, Font.NORMAL);

                    document.Add(new Paragraph("İş Listesi", titleFont));
                    document.Add(new Paragraph("Tarih: " + DateTime.Now.ToString("dd.MM.yyyy"), normalFont));
                    document.Add(new Chunk("\n"));

                    PdfPTable table = new PdfPTable(6);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 3, 3, 4, 2, 2, 2 });

                    table.AddCell(new PdfPCell(new Phrase("Müşteri", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("İş Başlığı", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Açıklama", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Durum", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Başlangıç", headerFont)));
                    table.AddCell(new PdfPCell(new Phrase("Bitiş", headerFont)));

                    foreach (Job j in jobs)
                    {
                        table.AddCell(new Phrase(j.CustomerFullName, normalFont));
                        table.AddCell(new Phrase(j.JobTitle, normalFont));
                        table.AddCell(new Phrase(j.Description, normalFont));
                        table.AddCell(new Phrase(GetEnumDescription(j.Status), normalFont));
                        table.AddCell(new Phrase(j.StartDate.HasValue ? j.StartDate.Value.ToString("dd.MM.yyyy") : "-", normalFont));
                        table.AddCell(new Phrase(j.EndDate.HasValue ? j.EndDate.Value.ToString("dd.MM.yyyy") : "-", normalFont));
                    }

                    document.Add(table);
                    document.Close();
                }
                return (true, "İş listesi PDF olarak dışa aktarıldı.");
            }
            catch (Exception ex)
            {
                return (false, "PDF oluşturma hatası: " + ex.Message);
            }
        }
    }
}