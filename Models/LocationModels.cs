using System.Collections.Generic;

namespace IsTakipWpf.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Districts { get; set; } = new List<string>();
    }
}
