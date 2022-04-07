using AndreAirlinesAPI3._0DapperConsoleApp.Service;
using AndreAirlinesAPI3._0Models;
using System;
using System.Collections.Generic;

namespace AndreAirlinesAPI3._0DapperConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AirportService airportService = new();

            Console.WriteLine("\n Ingestão de dados via Dapper");

            List<AirportData> airports = ReadData.ReturnAirports();

            airports.ForEach(airport =>
            {
                bool status = airportService.Add(airport);
                if (!status)
                {
                    Console.WriteLine("\n O dado abaixo não foi inserido: ");
                    Console.WriteLine(" City: " + airport.City);
                    Console.WriteLine(" Country: " + airport.Country);
                    Console.WriteLine(" Code: " + airport.Code);
                    Console.WriteLine(" Continent: " + airport.Continent);
                    Console.WriteLine("\n Pressione ENTER para continuar a ingestão dos outros dados...");
                    Console.ReadKey();
                }

            });

            Console.WriteLine("\n Ingestão terminada!");
        }
    }
}
