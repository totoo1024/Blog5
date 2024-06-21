using System.Collections.Generic;

namespace App.Hosting.Models
{
    public class TimeLineDto
    {
        public int Year { get; set; }

        public Dictionary<string, IEnumerable<LineItem>> Items { get; set; }
    }
}