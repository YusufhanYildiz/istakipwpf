using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public class ExcelService : IExcelService
    {
        public async Task<(bool Success, string Message, List<Customer> Data)> ImportCustomersAsync(string filePath)
        {
            try
            {
                var customers = new List<Customer>();
                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skip header

                    foreach (var row in rows)
                    {
                        customers.Add(new Customer
                        {
                            FirstName = row.Cell(1).GetValue<string>(),
                            LastName = row.Cell(2).GetValue<string>(),
                            PhoneNumber = row.Cell(3).GetValue<string>(),
                            Address = row.Cell(4).GetValue<string>()
                        });
                    }
                }
                return (true, $"{customers.Count} müşteri başarıyla okundu.", customers);
            }
            catch (Exception ex)
            {
                return (false, $"Excel okuma hatası: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message)> ExportCustomersAsync(string filePath, IEnumerable<Customer> customers)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Müşteriler");
                    worksheet.Cell(1, 1).Value = "Ad";
                    worksheet.Cell(1, 2).Value = "Soyad";
                    worksheet.Cell(1, 3).Value = "Telefon";
                    worksheet.Cell(1, 4).Value = "Adres";

                    int row = 2;
                    foreach (var c in customers)
                    {
                        worksheet.Cell(row, 1).Value = c.FirstName;
                        worksheet.Cell(row, 2).Value = c.LastName;
                        worksheet.Cell(row, 3).Value = c.PhoneNumber;
                        worksheet.Cell(row, 4).Value = c.Address;
                        row++;
                    }
                    workbook.SaveAs(filePath);
                }
                return (true, "Dışa aktarma başarılı.");
            }
            catch (Exception ex)
            {
                return (false, $"Excel yazma hatası: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message, List<Job> Data)> ImportJobsAsync(string filePath)
        {
            return (true, "İş içe aktarma henüz tam hazır değil.", new List<Job>());
        }

        public async Task<(bool Success, string Message)> ExportJobsAsync(string filePath, IEnumerable<Job> jobs)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("İşler");
                    worksheet.Cell(1, 1).Value = "Müşteri";
                    worksheet.Cell(1, 2).Value = "İş Başlığı";
                    worksheet.Cell(1, 3).Value = "Durum";
                    worksheet.Cell(1, 4).Value = "Başlangıç";
                    worksheet.Cell(1, 5).Value = "Bitiş";

                    int row = 2;
                    foreach (var j in jobs)
                    {
                        worksheet.Cell(row, 1).Value = j.CustomerFullName;
                        worksheet.Cell(row, 2).Value = j.JobTitle;
                        worksheet.Cell(row, 3).Value = j.Status.ToString();
                        worksheet.Cell(row, 4).Value = j.StartDate?.ToString("dd.MM.yyyy");
                        worksheet.Cell(row, 5).Value = j.EndDate?.ToString("dd.MM.yyyy");
                        row++;
                    }
                    workbook.SaveAs(filePath);
                }
                return (true, "Dışa aktarma başarılı.");
            }
            catch (Exception ex)
            {
                return (false, $"Excel yazma hatası: {ex.Message}");
            }
        }
    }
}