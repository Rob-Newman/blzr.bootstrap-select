using System.Collections.Generic;

namespace WebAssembly.Shared.Models
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Region { get; set; }

        public IEnumerable<string> AlternativeNames { get; set; }
    }
}
