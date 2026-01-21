using System;

namespace IsTakipWpf.Models
{
    public enum JobStatus
    {
        Bekliyor = 0,
        DevamEdiyor = 1,
        Tamamlandi = 2,
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

        // Navigation property for UI display
        public string CustomerFullName { get; set; }
    }
}
