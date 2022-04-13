using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Ticket.Service
{
    public class SearchFlight
    {
        public static async Task<Flight> ReturnFlight(Flight flightIn, string token)
        {
            HttpClient client = new HttpClient();
            Flight flight = new();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44310/api/Flights/" + flightIn.Id);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    flight = JsonConvert.DeserializeObject<Flight>(responseBody);
                    flight.ErrorCode = null;

                    return flight;
                }
                else
                {
                    flight.ErrorCode = response.StatusCode.ToString();

                    return flight;
                }

            }
            catch (HttpRequestException exception)
            {
                if (exception.StatusCode == null)
                    flight.ErrorCode = exception.InnerException.Message;
                else
                    flight.ErrorCode = exception.StatusCode.ToString();

                return flight;
            }
        }
    }
}
