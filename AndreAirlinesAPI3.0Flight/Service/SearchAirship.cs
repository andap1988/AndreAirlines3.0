using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Flight.Service
{
    public class SearchAirship
    {
        public static async Task<Airship> ReturnAirship(Airship airshipIn)
        {
            HttpClient client = new HttpClient();
            Airship airship = new();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44389/api/Airships/" + airshipIn.Id);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    airship = JsonConvert.DeserializeObject<Airship>(responseBody);
                    airship.ErrorCode = null;

                    return airship;
                }
                else
                {
                    airship.ErrorCode = response.StatusCode.ToString();

                    return airship;
                }

            }
            catch (HttpRequestException exception)
            {
                if (exception.StatusCode == null)
                    airship.ErrorCode = exception.InnerException.Message;
                else
                    airship.ErrorCode = exception.StatusCode.ToString();

                return airship;
            }
        }
    }
}
