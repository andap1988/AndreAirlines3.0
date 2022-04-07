using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Ticket.Service
{
    public class SearchBasePrice
    {
        public static async Task<BasePrice> ReturnBasePrice(BasePrice basePriceIn)
        {
            HttpClient client = new HttpClient();
            BasePrice basePrice = new();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44364/api/BasePrices/" + basePriceIn.Id);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    basePrice = JsonConvert.DeserializeObject<BasePrice>(responseBody);
                    basePrice.ErrorCode = null;

                    return basePrice;
                }
                else
                {
                    basePrice.ErrorCode = response.StatusCode.ToString();

                    return basePrice;
                }

            }
            catch (HttpRequestException exception)
            {
                if (exception.StatusCode == null)
                    basePrice.ErrorCode = exception.InnerException.Message;
                else
                    basePrice.ErrorCode = exception.StatusCode.ToString();

                return basePrice;
            }
        }
    }
}
