using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Airport.Service
{
    public class SearchAirportData
    {
        public static async Task<AirportData> ReturnAirportData(string iataCode, string token)
        {
            HttpClient client = new HttpClient();
            AirportData aiportData = new();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44310/api/AiportsData/code/" + iataCode);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    aiportData = JsonConvert.DeserializeObject<AirportData>(responseBody);
                    aiportData.ErrorCode = null;

                    return aiportData;
                }
                else
                {
                    aiportData.ErrorCode = response.StatusCode.ToString();

                    return aiportData;
                }

            }
            catch (HttpRequestException exception)
            {
                if (exception.StatusCode == null)
                    aiportData.ErrorCode = exception.InnerException.Message;
                else
                    aiportData.ErrorCode = exception.StatusCode.ToString();

                return aiportData;
            }
        }
    }
}
