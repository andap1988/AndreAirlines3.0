using Microsoft.EntityFrameworkCore;

namespace AndreAirlinesAPI3._0Frontend.Models
{
    [Keyless]
    public class Address
    {
        public string District { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public string Continent { get; set; }
        public string ErrorCode { get; set; }
    }
}
