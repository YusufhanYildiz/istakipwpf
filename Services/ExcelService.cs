using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using ClosedXML.Excel;
using ExcelDataReader;
using IsTakipWpf.Models;
using System.Text;

namespace IsTakipWpf.Services
{
    public class ExcelService : IExcelService
    {
        private string GetEnumDescription(Enum value)
        {
            if (value == null) return string.Empty;
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr != null ? attr.Description : value.ToString();
        }

        private async Task<DataTable> ReadExcelAsDataTableAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    // ExcelDataReader otomatik olarak formatı (xls, xlsx, csv) algılar
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true // İlk satırı başlık olarak kullan
                            }
                        });
                        return result.Tables[0];
                    }
                }
            });
        }

        public async Task<(bool Success, string Message, List<Customer> Data)> ImportCustomersAsync(string filePath)
        {
            try
            {
                var dt = await ReadExcelAsDataTableAsync(filePath);
                var customers = new List<Customer>();

                foreach (DataRow row in dt.Rows)
                {
                    customers.Add(new Customer
                    {
                        FirstName = row[0]?.ToString()?.Trim(),
                        LastName = row[1]?.ToString()?.Trim(),
                        PhoneNumber = row[2]?.ToString()?.Trim(),
                        Address = row[3]?.ToString()?.Trim(),
                        City = row[4]?.ToString()?.Trim(),
                        District = row[5]?.ToString()?.Trim()
                    });
                }
                return (true, $"{customers.Count} müşteri başarıyla okundu.", customers);
            }
            catch (Exception ex)
            {
                return (false, "Veri okuma hatası: " + ex.Message, null);
            }
        }

        public async Task<(bool Success, string Message, List<Job> Data)> ImportJobsAsync(string filePath)
        {
            try
            {
                // .NET Framework'te ExcelDataReader iÃ§in encoding kaydÄ± gerekebilir
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                var dt = await ReadExcelAsDataTableAsync(filePath);
                var jobs = new List<Job>();

                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] == null || string.IsNullOrWhiteSpace(row[0].ToString())) continue;

                    var statusStr = row[5]?.ToString()?.Trim() ?? string.Empty;
                    var status = JobStatus.Bekliyor;

                    if (!string.IsNullOrEmpty(statusStr))
                    {
                        if (statusStr.IndexOf("Devam", StringComparison.OrdinalIgnoreCase) >= 0) status = JobStatus.DevamEdiyor;
                        else if (statusStr.IndexOf("Tamam", StringComparison.OrdinalIgnoreCase) >= 0) status = JobStatus.Tamamlandi;
                        else if (statusStr.IndexOf("İptal", StringComparison.OrdinalIgnoreCase) >= 0) status = JobStatus.IptalEdildi;
                    }

                    var job = new Job
                    {
                        CustomerFullName = row[0]?.ToString()?.Trim(),
                        CustomerCity = row[1]?.ToString()?.Trim(),
                        CustomerDistrict = row[2]?.ToString()?.Trim(),
                        JobTitle = row[3]?.ToString()?.Trim(),
                        Description = row[4]?.ToString()?.Trim(),
                        Status = status
                    };

                    // Tarih ve Sayısal değerlerin güvenli okunması
                    if (DateTime.TryParse(row[6]?.ToString(), out DateTime startDate)) job.StartDate = startDate;
                    if (DateTime.TryParse(row[7]?.ToString(), out DateTime endDate)) job.EndDate = endDate;

                    string priceStr = row[8]?.ToString()?.Replace("₺", "").Trim();
                    if (decimal.TryParse(priceStr, out decimal price)) job.Price = price;

                    string paidStr = row[9]?.ToString()?.Replace("₺", "").Trim();
                    if (decimal.TryParse(paidStr, out decimal paid)) job.PaidAmount = paid;

                    jobs.Add(job);
                }
                return (true, $"{jobs.Count} iş başarıyla okundu.", jobs);
            }
            catch (Exception ex)
            {
                return (false, "Veri okuma hatası: " + ex.Message, null);
            }
        }

        public async Task<(bool Success, string Message)> ExportCustomersAsync(string filePath, IEnumerable<Customer> customers)
        {
            try
            {
                string extension = Path.GetExtension(filePath).ToLower();
                if (extension == ".csv")
                {
                    return await ExportToCsvAsync(filePath, new[] { "Ad", "Soyad", "Telefon", "Adres", "İl", "İlçe" }, 
                        customers.Select(c => new[] { c.FirstName, c.LastName, c.PhoneNumber, c.Address, c.City, c.District }));
                }

                return await Task.Run(() =>
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Müşteriler");
                        string[] headers = { "Ad", "Soyad", "Telefon", "Adres", "İl", "İlçe" };
                        for (int i = 0; i < headers.Length; i++) worksheet.Cell(1, i + 1).Value = headers[i];

                        int row = 2;
                        foreach (var c in customers)
                        {
                            worksheet.Cell(row, 1).Value = c.FirstName;
                            worksheet.Cell(row, 2).Value = c.LastName;
                            worksheet.Cell(row, 3).Value = c.PhoneNumber;
                            worksheet.Cell(row, 4).Value = c.Address;
                            worksheet.Cell(row, 5).Value = c.City;
                            worksheet.Cell(row, 6).Value = c.District;
                            row++;
                        }
                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(filePath);
                    }
                    return (true, "Dışa aktarma başarılı.");
                });
            }
            catch (Exception ex)
            {
                return (false, "Hata: " + ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> ExportJobsAsync(string filePath, IEnumerable<Job> jobs)
        {
            try
            {
                string extension = Path.GetExtension(filePath).ToLower();
                string[] headers = { "Müşteri", "İl", "İlçe", "İş Başlığı", "Açıklama", "Durum", "Başlangıç", "Bitiş", "İş Tutarı", "Alınan Ödeme", "Kalan Bakiye" };

                if (extension == ".csv")
                {
                    return await ExportToCsvAsync(filePath, headers, jobs.Select(j => new[] {
                        j.CustomerFullName, j.CustomerCity, j.CustomerDistrict, j.JobTitle, j.Description,
                        GetEnumDescription(j.Status), j.StartDate?.ToShortDateString(), j.EndDate?.ToShortDateString(),
                        j.Price.ToString(), j.PaidAmount.ToString(), j.Balance.ToString()
                    }));
                }

                return await Task.Run(() =>
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("İşler");
                        for (int i = 0; i < headers.Length; i++) worksheet.Cell(1, i + 1).Value = headers[i];

                        int row = 2;
                        foreach (var j in jobs)
                        {
                            worksheet.Cell(row, 1).Value = j.CustomerFullName;
                            worksheet.Cell(row, 2).Value = j.CustomerCity;
                            worksheet.Cell(row, 3).Value = j.CustomerDistrict;
                            worksheet.Cell(row, 4).Value = j.JobTitle;
                            worksheet.Cell(row, 5).Value = j.Description;
                            worksheet.Cell(row, 6).Value = GetEnumDescription(j.Status);
                            if (j.StartDate.HasValue)
                            {
                                worksheet.Cell(row, 7).Value = j.StartDate.Value;
                                worksheet.Cell(row, 7).Style.DateFormat.Format = "dd.MM.yyyy";
                            }
                            if (j.EndDate.HasValue)
                            {
                                worksheet.Cell(row, 8).Value = j.EndDate.Value;
                                worksheet.Cell(row, 8).Style.DateFormat.Format = "dd.MM.yyyy";
                            }
                            worksheet.Cell(row, 9).Value = j.Price;
                            worksheet.Cell(row, 10).Value = j.PaidAmount;
                            worksheet.Cell(row, 11).Value = j.Balance;
                            row++;
                        }
                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(filePath);
                    }
                    return (true, "Dışa aktarma başarılı.");
                });
            }
            catch (Exception ex)
            {
                return (false, "Hata: " + ex.Message);
            }
        }

        private async Task<(bool Success, string Message)> ExportToCsvAsync(string filePath, string[] headers, IEnumerable<string[]> rows)
        {
            return await Task.Run(() =>
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Join(";", headers)); // Excel dostu ayracı (noktalı virgül) kullanıyoruz
                foreach (var row in rows)
                {
                    sb.AppendLine(string.Join(";", row.Select(r => $"\"{r?.Replace("\"", "\"\"")}\"")));
                }
                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                return (true, "CSV dışa aktarma başarılı.");
            });
        }
    }
}
