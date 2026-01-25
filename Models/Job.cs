using System;
using System.ComponentModel;

namespace IsTakipWpf.Models
{
    public enum JobStatus
    {
        [Description("Bekliyor")]
        Bekliyor = 0,
        [Description("Devam Ediyor")]
        DevamEdiyor = 1,
        [Description("Tamamlandı")]
        Tamamlandi = 2,
        [Description("İptal Edildi")]
        IptalEdildi = 3
    }

    public class Job
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public JobStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public decimal Price { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Balance => PaidAmount - Price;

        public string CustomerFullName { get; set; }

                        public string CustomerCity { get; set; }
                
                        public string CustomerDistrict { get; set; }
                
                        public string CustomerLocation => $"{CustomerCity} / {CustomerDistrict}";
                    }
                }
        