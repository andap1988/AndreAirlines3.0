using AndreAirlinesAPI3._0Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0RabbitMQ.Service
{
    public class SenderLogMongoService
    {
        static readonly HttpClient client = new();

        public static async Task Add(Log log)
        {
            try
            {
                if (client.BaseAddress == null) client.BaseAddress = new Uri("https://localhost:44370/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                await client.PostAsJsonAsync("api/LogsToMongo", log);
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine("\n Exception Caught!\n");
                Console.WriteLine(" Message -> " + exception.Message);
            }
        }
    }
}
