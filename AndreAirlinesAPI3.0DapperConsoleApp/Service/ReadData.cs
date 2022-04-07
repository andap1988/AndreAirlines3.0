using AndreAirlinesAPI3._0Models;
using System.Collections.Generic;
using System.IO;

namespace AndreAirlinesAPI3._0DapperConsoleApp.Service
{
    public class ReadData
    {
        public static List<AirportData> ReturnAirports()
        {
            List<AirportData> airports = new();

            var archive = GetPathArchiveAiportDataCsv();

            using (var reader = new StreamReader(archive))
            {
                var line = reader.ReadLine();

                while (line != null)
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = line.Split(';');
                        airports.Add(new AirportData(values[0], values[1], values[2], values[3]));
                    }
                }
            }

            return airports;
        }

        public static string GetPathArchiveAiportDataCsv()
        {
            string path = Directory.GetCurrentDirectory() + "\\File\\Dados.csv";

            return path;
        }
    }
}
