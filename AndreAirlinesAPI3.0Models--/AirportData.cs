using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Models
{
    public class AirportData
    {
        public readonly static string INSERT = "INSERT INTO Airport (City, Country, Code, Continent) VALUES (@City, @Country, @Code, @Continent)";
        public readonly static string GETALL = "SELECT Id, City, Country, Code, Continent FROM Airport";
        public readonly static string GETONE = "SELECT Id, City, Country, Code, Continent FROM Airport WHERE Id = @Id";

        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string Continent { get; set; }
        public string ErrorCode { get; set; }

        public AirportData(string city, string country, string code, string continent)
        {
            City = city;
            Country = country;
            Code = code;
            Continent = continent;
        }

        public AirportData() { }

        public override string ToString()
        {
            return " Id: " + Id
                + "\n City: " + City
                + "\n Country: " + Country
                + "\n Code: " + Code
                + "\n Continent: " + Continent;
        }
    }
}
