using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0BasePrice.Service
{
    public class SearchAirport
    {
        public static async Task<Airport> ReturnAirport(Airport airportIn, bool isIataCode)
        {
            HttpClient client = new HttpClient();
            Airport airport = new();
            string composeLink;

            if (isIataCode)
                composeLink = "Airport/code/" + airportIn.IataCode;
            else
                composeLink = "Airport/" + airportIn.Id;

            try
            {

                HttpResponseMessage response = await client.GetAsync("https://localhost:44304/api/" + composeLink);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    airport = JsonConvert.DeserializeObject<Airport>(responseBody);
                    airport.ErrorCode = null;

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
