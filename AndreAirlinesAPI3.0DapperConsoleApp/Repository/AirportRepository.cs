using AndreAirlinesAPI3._0DapperConsoleApp.Config;
using AndreAirlinesAPI3._0Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0DapperConsoleApp.Repository
{
    public class AirportRepository : IAirportRepository
    {
        private string _conn;

        public AirportRepository()
        {
            _conn = DataBaseConfiguration.Get();
        }

        public bool Add(AirportData airport)
        {
            bool status = false;

            try
            {
                using (var conn = new SqlConnection(_conn))
                {
                    conn.Open();
                    conn.Execute(AirportData.INSERT, airport);
                    status = true;
                }

                return status;
            }
            catch (Exception exception)
            {
                return status;
            }


        }

        public List<AirportData> GetAll()
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var airports = conn.Query<AirportData>(AirportData.GETALL);

                return (List<AirportData>)airports;
            }
        }
    }
}
