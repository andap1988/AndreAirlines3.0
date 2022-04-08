using System.ComponentModel.DataAnnotations.Schema;

namespace AndreAirlinesAPI3._0Frontend.Models
{
    [Table("Airport")]
    public class Airport
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string Continent { get; set; }
    }
}
