using AndreAirlinesAPI3._0Models;
using System.Collections.Generic;

namespace AndreAirlinesAPI3._0Dapper.Repository
{
    public interface IAirportRepository
    {
        List<AirportData> GetAll();

        AirportData GetOne(int id);
    }
}
