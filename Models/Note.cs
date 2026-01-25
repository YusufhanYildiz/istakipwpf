using System;

namespace IsTakipWpf.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Color { get; set; } = "#FFFFFF"; // Default White
        public bool IsPinned { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
