using AndreAirlinesAPI3._0Dapper.Context;
using AndreAirlinesAPI3._0Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace AndreAirlinesAPI3._0Dapper.Repository
{
    public class AirportRepository : IAirportRepository
    {
        private readonly DapperContext _context;

        public AirportRepository(DapperContext context)
        {
            _context = context;
        }

        public AirportData GetOne(int id)
        {
            AirportData airportData = new();

            try
            {
                using (var conn = _context.CreateConnection())
                {
                    conn.Open();
                    var airport = conn.QueryFirstOrDefault<AirportData>($"SELECT Id, City, Country, Code, Continent FROM Airport WHERE Id = {id}");

                    return airport;
                }
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    airportData.ErrorCode = exception.InnerException.Message;
                else
                    airportData.ErrorCode = exception.Message.ToString();

                return airportData;
            }
        }

        public List<AirportData> GetAll()
        {
            List<AirportData> aiportsData = new();

            try
            {
                using (var conn = _context.CreateConnection())
                {
                    conn.Open();
                    var airports = conn.Query<AirportData>(AirportData.GETALL);

                    return (List<AirportData>)airports;
                }
            }
            catch (Exception exception)
            {
                aiportsData.Add(new AirportData());

                if (exception.InnerException != null)
                    aiportsData[0].ErrorCode = exception.InnerException.Message;
                else
                    aiportsData[0].ErrorCode = exception.Message.ToString();

                return aiportsData;
            }
        }

        public AirportData GetAiportData(string iatacode)
        {
            AirportData airportData = new();

            try
            {
                using (var conn = _context.CreateConnection())
                {
                    conn.Open();
                    var airport = conn.QueryFirstOrDefault<AirportData>($"SELECT Id, City, Country, Code, Continent FROM Airport WHERE Code = '{iatacode.ToUpper()}'");

                    return airport;
                }
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    airportData.ErrorCode = exception.InnerException.Message;
                else
                    airportData.ErrorCode = exception.Message.ToString();

                return airportData;
            }
        }
    }
}
