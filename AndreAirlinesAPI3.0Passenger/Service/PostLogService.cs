using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Passenger.Service
{
    public class PostLogService
    {
        public static async Task<string> InsertLog(Log log)
        {
            HttpClient client = new HttpClient();

            try
            {
                var json = JsonConvert.SerializeObject(log);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await client.PostAsync("https://localhost:44344/api/Logs", content);
                result.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    return "ok";
                else
                    return "notOk";
            }
            catch (HttpRequestException exception)
            {
                return "notOk";
            }
        }
    }
}
