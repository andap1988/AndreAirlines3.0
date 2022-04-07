using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0DapperVsEFConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool internFlag = true, externFlag = true;

            do
            {
                Console.Clear();
                Console.WriteLine("\n Teste de Performance");
                Console.WriteLine("\n Dapper VS EF");

                Thread.Sleep(2000);

                Console.WriteLine("\n Primeiro teste -> Dapper");

                Console.WriteLine("\n Começando em 3s...");

                Thread.Sleep(3000);

                var beginDapper = DateTime.Now;

                Console.WriteLine("\n Iniciando em -> " + beginDapper.ToString());

                for (int i = 1; i < 101; i++)
                {
                    Console.WriteLine($" Consulta {i} -> " + DateTime.Now);
                    ConsultAPIDapper(i).Wait();
                }

                var endDapper = DateTime.Now;
                var differenceDapper = endDapper - beginDapper;
                Console.WriteLine("\n Terminando em -> " + endDapper.ToString());
                Console.WriteLine(" Diferença -> " + differenceDapper);
                Console.WriteLine("\n Pressione ENTER para a próxima etapa...");
                Console.ReadKey();

                Console.Clear();

                Console.WriteLine("\n Teste de Performance");
                Console.WriteLine("\n Dapper VS EF");

                Thread.Sleep(2000);

                Console.WriteLine("\n Segundo teste -> EF");

                Console.WriteLine("\n Começando em 3s...");

                Thread.Sleep(3000);

                var beginEF = DateTime.Now;

                Console.WriteLine("\n Iniciando em -> " + beginEF.ToString());

                for (int i = 1; i < 101; i++)
                {
                    Console.WriteLine($" Consulta {i} -> " + DateTime.Now);
                    ConsultAPIEF(i).Wait();
                }

                var endEF = DateTime.Now;
                var differenceEF = endEF - beginEF;
                Console.WriteLine("\n Terminando em -> " + endEF.ToString());
                Console.WriteLine(" Diferença -> " + differenceEF);
                Console.WriteLine("\n Pressione ENTER ver o resultado final...");
                Console.ReadKey();

                Console.Clear();

                Console.WriteLine("\n Teste de Performance");
                Console.WriteLine("\n Dapper VS EF");

                Thread.Sleep(2000);

                Console.WriteLine("\n Dapper -> " + differenceDapper);
                Console.WriteLine("\n EF -> " + differenceEF);

                var differenceFinal = differenceDapper - differenceEF;

                if (differenceDapper > differenceEF)
                    Console.WriteLine($"\n EF se saiu melhor por {differenceDapper - differenceEF}");
                else
                    Console.WriteLine($"\n Dapper se saiu melhor por {differenceEF - differenceDapper}");

                do
                {
                    Console.WriteLine("\n Deseja repetir o teste?");
                    Console.WriteLine("\n 1 - SIM / 2 - NÃO");
                    Console.Write("\n Escolha: ");
                    string choice = Console.ReadLine();
                    int.TryParse(choice, out int option);

                    if (option < 1 || option > 2)
                    {
                        Console.WriteLine("\n Opção inválida");
                        Console.WriteLine("\n Pressione ENTER para digitar novamente...");
                        Console.ReadKey();
                    }
                    else if (option == 2)
                    {
                        internFlag = false;
                        externFlag = false;
                    }
                    else
                        internFlag = false;

                } while (internFlag);

            } while (externFlag);

            Console.WriteLine("\n\n Até a próxima!\n\n\n\n\n\n\n\n");
        }

        public static async Task<AirportData> ConsultAPIDapper(int id)
        {
            HttpClient client = new HttpClient();
            AirportData airport = new();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44371/api/AiportsData/" + id);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    airport = JsonConvert.DeserializeObject<AirportData>(responseBody);
                    airport.ErrorCode = null;
                    Console.WriteLine(" Code: " + airport.Code);

                    return airport;
                }
                else
                {
                    airport.ErrorCode = response.StatusCode.ToString();

                    return airport;
                }

            }
            catch (HttpRequestException exception)
            {
                if (exception.StatusCode == null)
                    airport.ErrorCode = exception.InnerException.Message;
                else
                    airport.ErrorCode = exception.StatusCode.ToString();

                return airport;
            }
        }

        public static async Task<AirportData> ConsultAPIEF(int id)
        {
            HttpClient client = new HttpClient();
            AirportData airport = new();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44325/api/Airports/" + id);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    airport = JsonConvert.DeserializeObject<AirportData>(responseBody);
                    airport.ErrorCode = null;
                    Console.WriteLine(" Code: " + airport.Code);

                    return airport;
                }
                else
                {
                    airport.ErrorCode = response.StatusCode.ToString();

                    return airport;
                }

            }
            catch (HttpRequestException exception)
            {
                if (exception.StatusCode == null)
                    airport.ErrorCode = exception.InnerException.Message;
                else
                    airport.ErrorCode = exception.StatusCode.ToString();

                return airport;
            }
        }
    }
}
