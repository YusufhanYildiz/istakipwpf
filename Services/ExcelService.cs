using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using ClosedXML.Excel;
using IsTakipWpf.Models;

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

        public async Task<(bool Success, string Message, List<Customer> Data)> ImportCustomersAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var customers = new List<Customer>();
                    using (var workbook = new XLWorkbook(filePath))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                        foreach (var row in rows)
                        {
                            customers.Add(new Customer
                            {
                                FirstName = row.Cell(1).GetValue<string>(),
                                LastName = row.Cell(2).GetValue<string>(),
                                PhoneNumber = row.Cell(3).GetValue<string>(),
                                Address = row.Cell(4).GetValue<string>(),
                                City = row.Cell(5).GetValue<string>(),
                                District = row.Cell(6).GetValue<string>()
                            });
                        }
                    }
                    return (true, customers.Count + " müşteri başarıyla okundu.", customers);
                }
                catch (Exception ex)
                {
                    return (false, "Excel okuma hatası: " + ex.Message, null);
                }
            });
        }

        public async Task<(bool Success, string Message)> ExportCustomersAsync(string filePath, IEnumerable<Customer> customers)
        {
            return await Task.Run(() =>
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
                        worksheet.Cell(1, 5).Value = "İl";
                        worksheet.Cell(1, 6).Value = "İlçe";

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
                }
                catch (Exception ex)
                {
                    return (false, "Excel yazma hatası: " + ex.Message);
                }
            });
        }

        public async Task<(bool Success, string Message, List<Job> Data)> ImportJobsAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var jobs = new List<Job>();
                    using (var workbook = new XLWorkbook(filePath))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                        foreach (var row in rows)
                        {
                            var job = new Job
                            {
                                CustomerFullName = row.Cell(1).GetValue<string>(),
                                JobTitle = row.Cell(4).GetValue<string>(),
                                Description = row.Cell(5).GetValue<string>(),
                                Status = JobStatus.Bekliyor
                            };

                            // Başlangıç Tarihi Oku
                            var startCell = row.Cell(7);
                            if (!startCell.IsEmpty())
                            {
                                if (startCell.DataType == XLDataType.DateTime)
                                    job.StartDate = startCell.GetDateTime();
                                else if (DateTime.TryParse(startCell.GetValue<string>(), out DateTime dt))
                                    job.StartDate = dt;
                            }

                            // Bitiş Tarihi Oku
                            var endCell = row.Cell(8);
                            if (!endCell.IsEmpty())
                            {
                                if (endCell.DataType == XLDataType.DateTime)
                                    job.EndDate = endCell.GetDateTime();
                                else if (DateTime.TryParse(endCell.GetValue<string>(), out DateTime dt))
                                    job.EndDate = dt;
                            }

                            jobs.Add(job);
                        }
                    }
                    return (true, jobs.Count + " iş başarıyla okundu.", jobs);
                }
                catch (Exception ex)
                {
                    return (false, "Excel okuma hatası: " + ex.Message, null);
                }
            });
        }

        public async Task<(bool Success, string Message)> ExportJobsAsync(string filePath, IEnumerable<Job> jobs)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("İşler");
                        worksheet.Cell(1, 1).Value = "Müşteri";
                        worksheet.Cell(1, 2).Value = "İl";
                        worksheet.Cell(1, 3).Value = "İlçe";
                        worksheet.Cell(1, 4).Value = "İş Başlığı";
                        worksheet.Cell(1, 5).Value = "Açıklama";
                        worksheet.Cell(1, 6).Value = "Durum";
                        worksheet.Cell(1, 7).Value = "Başlangıç";
                        worksheet.Cell(1, 8).Value = "Bitiş";

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
                            row++;
                        }
                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(filePath);
                    }
                    return (true, "Dışa aktarma başarılı.");
                }
                catch (Exception ex)
                {
                    return (false, "Excel yazma hatası: " + ex.Message);
                }
            });
        }
    }
}