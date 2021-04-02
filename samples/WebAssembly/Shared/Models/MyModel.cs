using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAssembly.Shared.Models
{
    public class MyModel
    {
        public MyModel()
        {
            SelectedIntValues = new List<int>();
            SelectedStringValues = new List<string>();
        }

        public IEnumerable<int> SelectedIntValues { get; set; }

        public IEnumerable<string> SelectedStringValues { get; set; }

        [Required]
        public string SelectedStringValue { get; set; }


        public int SelectedIntValue { get; set; }
    }
}
