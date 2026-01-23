using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public class ReportingService : IReportingService
    {
        private readonly string _accentColor = "#2c3e50"; // Deep Slate
        private readonly string _secondaryColor = "#f8f9fa"; // Light Silver

        public ReportingService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        private string GetEnumDescription(Enum value)
        {
            if (value == null) return string.Empty;
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr != null ? attr.Description : value.ToString();
        }

        private byte[] GetLogo()
        {
            // First check execution directory, then project root as fallback
            string[] paths = {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "logo.png"),
                "logo.png"
            };

            foreach (var path in paths)
            {
                if (File.Exists(path)) return File.ReadAllBytes(path);
            }
            return null;
        }

        public async Task<(bool Success, string Message)> GenerateCustomerHistoryPdfAsync(string filePath, Customer customer, IEnumerable<Job> jobs)
        {
            try
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        PreparePage(page, "MÜŞTERİ İŞ GEÇMİŞİ");

                        page.Content().Column(col =>
                        {
                            col.Item().PaddingBottom(20).Row(row =>
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("Müşteri Detayları").FontSize(10).FontColor(Colors.Grey.Medium);
                                    c.Item().Text(customer.FullName).FontSize(18).SemiBold().FontColor(_accentColor);
                                    if (!string.IsNullOrEmpty(customer.PhoneNumber))
                                        c.Item().Text(customer.PhoneNumber).FontSize(10).FontColor(Colors.Grey.Darken2);
                                });
                            });
                            
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(HeaderStyle).Text("TARİH");
                                    header.Cell().Element(HeaderStyle).Text("İŞ BAŞLIĞI");
                                    header.Cell().Element(HeaderStyle).Text("AÇIKLAMA");
                                    header.Cell().Element(HeaderStyle).Text("DURUM");
                                    header.Cell().Element(HeaderStyle).Text("BİTİŞ");
                                });

                                bool isOdd = false;
                                foreach (var job in jobs)
                                {
                                    bool currentIsOdd = isOdd;
                                    table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(job.CreatedDate.ToString("dd.MM.yyyy"));
                                    table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(job.JobTitle);
                                    table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(job.Description);
                                    table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(GetEnumDescription(job.Status));
                                    table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(job.EndDate?.ToString("dd.MM.yyyy") ?? "-");
                                    isOdd = !isOdd;
                                }
                            });
                        });
                    });
                }).GeneratePdf(filePath);

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
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        PreparePage(page, "MÜŞTERİ LİSTESİ", true);

                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2.5f);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(4);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderStyle).Text("AD");
                                header.Cell().Element(HeaderStyle).Text("SOYAD");
                                header.Cell().Element(HeaderStyle).Text("TELEFON");
                                header.Cell().Element(HeaderStyle).Text("İL");
                                header.Cell().Element(HeaderStyle).Text("İLÇE");
                                header.Cell().Element(HeaderStyle).Text("ADRES");
                            });

                            bool isOdd = false;
                            foreach (var c in customers)
                            {
                                bool currentIsOdd = isOdd;
                                table.Cell().Element(c1 => RowStyle(c1, currentIsOdd)).Text(c.FirstName ?? "");
                                table.Cell().Element(c1 => RowStyle(c1, currentIsOdd)).Text(c.LastName ?? "");
                                table.Cell().Element(c1 => RowStyle(c1, currentIsOdd)).Text(c.PhoneNumber ?? "");
                                table.Cell().Element(c1 => RowStyle(c1, currentIsOdd)).Text(c.City ?? "");
                                table.Cell().Element(c1 => RowStyle(c1, currentIsOdd)).Text(c.District ?? "");
                                table.Cell().Element(c1 => RowStyle(c1, currentIsOdd)).Text(c.Address ?? "");
                                isOdd = !isOdd;
                            }
                        });
                    });
                }).GeneratePdf(filePath);

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
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        PreparePage(page, "GENEL İŞ LİSTESİ", true);

                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(4);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderStyle).Text("MÜŞTERİ");
                                header.Cell().Element(HeaderStyle).Text("İL");
                                header.Cell().Element(HeaderStyle).Text("İLÇE");
                                header.Cell().Element(HeaderStyle).Text("İŞ BAŞLIĞI");
                                header.Cell().Element(HeaderStyle).Text("AÇIKLAMA");
                                header.Cell().Element(HeaderStyle).Text("DURUM");
                                header.Cell().Element(HeaderStyle).Text("BAŞLANGIÇ");
                                header.Cell().Element(HeaderStyle).Text("BİTİŞ");
                            });

                            bool isOdd = false;
                            foreach (var j in jobs)
                            {
                                bool currentIsOdd = isOdd;
                                table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(j.CustomerFullName ?? "");
                                table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(j.CustomerCity ?? "");
                                table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(j.CustomerDistrict ?? "");
                                table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(j.JobTitle ?? "");
                                table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(j.Description ?? "");
                                table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(GetEnumDescription(j.Status));
                                table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(j.StartDate?.ToString("dd.MM.yyyy") ?? "-");
                                table.Cell().Element(c => RowStyle(c, currentIsOdd)).Text(j.EndDate?.ToString("dd.MM.yyyy") ?? "-");
                                isOdd = !isOdd;
                            }
                        });
                    });
                }).GeneratePdf(filePath);

                return (true, "İş listesi PDF olarak dışa aktarıldı.");
            }
            catch (Exception ex)
            {
                return (false, "PDF oluşturma hatası: " + ex.Message);
            }
        }

        private void PreparePage(PageDescriptor page, string title, bool isLandscape = false)
        {
            page.Size(isLandscape ? PageSizes.A4.Landscape() : PageSizes.A4);
            page.Margin(1.5f, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(9).FontFamily(Fonts.SegoeUI));

            page.Header().PaddingBottom(20).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text(title).FontSize(26).ExtraBold().FontColor(_accentColor).LetterSpacing(0.05f);
                    col.Item().PaddingTop(-5).Height(3).Background(_accentColor).Width(60); // Accent line
                    col.Item().PaddingTop(5).Text($"{DateTime.Now:dd MMMM yyyy | HH:mm}").FontSize(9).FontColor(Colors.Grey.Medium);
                });

                var logo = GetLogo();
                if (logo != null)
                {
                    row.ConstantItem(80).AlignRight().Image(logo);
                }
            });

            page.Footer().PaddingTop(10).BorderTop(0.5f).BorderColor(Colors.Grey.Lighten2).Row(row =>
            {
                row.RelativeItem().Text("İş Takip Sistemi v1.0 | Profesyonel Raporlama").FontSize(8).FontColor(Colors.Grey.Medium);
                row.RelativeItem().AlignRight().Text(x =>
                {
                    x.Span("SAYFA ").FontSize(8).FontColor(Colors.Grey.Medium);
                    x.CurrentPageNumber().FontSize(8).SemiBold();
                    x.Span(" / ").FontSize(8).FontColor(Colors.Grey.Medium);
                    x.TotalPages().FontSize(8).SemiBold();
                });
            });
        }

        private QuestPDF.Infrastructure.IContainer HeaderStyle(QuestPDF.Infrastructure.IContainer container)
        {
            return container.DefaultTextStyle(x => x.SemiBold().FontSize(8).FontColor(Colors.White))
                            .PaddingVertical(8)
                            .PaddingHorizontal(8)
                            .Background(_accentColor)
                            .AlignLeft();
        }

        private QuestPDF.Infrastructure.IContainer RowStyle(QuestPDF.Infrastructure.IContainer container, bool isOdd)
        {
            var styles = container.BorderBottom(0.5f)
                                  .BorderColor(Colors.Grey.Lighten3)
                                  .PaddingVertical(6)
                                  .PaddingHorizontal(8)
                                  .AlignLeft();

            if (isOdd) styles = styles.Background(_secondaryColor);
            
            return styles;
        }
    }
}