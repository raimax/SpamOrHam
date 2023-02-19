using CsvHelper.Configuration.Attributes;
using SpamOrHam.Enums;

namespace SpamOrHam.Models
{
    public class CsvDataRow
    {
        [Index(0)]
        public string Content { get; set; }
        [Index(1)]
        public Classification Classification { get; set; }
    }
}
