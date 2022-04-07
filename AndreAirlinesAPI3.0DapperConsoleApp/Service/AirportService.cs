using AndreAirlinesAPI3._0DapperConsoleApp.Repository;
using AndreAirlinesAPI3._0Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0DapperConsoleApp.Service
{
    public class AirportService
    {
        private IAirportRepository _airportRepository;

        public AirportService()
        {
            _airportRepository = new AirportRepository();
        }

        public bool Add(AirportData airport)
        {
            return _airportRepository.Add(airport);
        }

        public List<AirportData> GetAll()
        {
            return _airportRepository.GetAll();
        }
    }
}
