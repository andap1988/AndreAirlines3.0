using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Ticket.Service
{
    public class SearchPassenger
    {
        public static async Task<Passenger> ReturnPassenger(Passenger passengerIn, bool isCpf)
        {
            HttpClient client = new HttpClient();
            Passenger passenger = new();
            string composeLink;

            if (isCpf)
                composeLink = "Passengers/cpf/" + passengerIn.Cpf;
            else
                composeLink = "Passengers/" + passengerIn.Id;

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44338/api/" + composeLink);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    passenger = JsonConvert.DeserializeObject<Passenger>(responseBody);
                    passenger.ErrorCode = null;

                    return passenger;
                }
                else
                {
                    passenger.ErrorCode = response.StatusCode.ToString();

                    return passenger;
                }

            }
            catch (HttpRequestException exception)
            {
                if (exception.StatusCode == null)
                    passenger.ErrorCode = exception.InnerException.Message;
                else
                    passenger.ErrorCode = exception.StatusCode.ToString();

                return passenger;
            }
        }
    }
}
