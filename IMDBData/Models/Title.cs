using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IMDBData.Models
{
    public class Title
    {
        public string Tconst { get; set; }
        public string? PrimaryTitle  { get; set; }
        public string? OriginalTitle { get; set; }
        public bool? IsAdult { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }
    }
}
