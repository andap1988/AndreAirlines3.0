using AndreAirlinesAPI3._0Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0DapperConsoleApp.Repository
{
    public interface IAirportRepository
    {
        bool Add(AirportData airport);

        List<AirportData> GetAll();
    }
}
