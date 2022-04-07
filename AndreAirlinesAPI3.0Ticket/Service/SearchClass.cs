using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Ticket.Service
{
    public class SearchClass
    {
        public static async Task<Class> ReturnClass(Class classIn)
        {
            HttpClient client = new HttpClient();
            Class classs = new();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44345/api/Classes/" + classIn.Id);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    classs = JsonConvert.DeserializeObject<Class>(responseBody);
                    classs.ErrorCode = null;

                    return classs;
                }
                else
                {
                    classs.ErrorCode = response.StatusCode.ToString();

                    return classs;
                }

            }
            catch (HttpRequestException exception)
            {
                if (exception.StatusCode == null)
                    classs.ErrorCode = exception.InnerException.Message;
                else
                    classs.ErrorCode = exception.StatusCode.ToString();

                return classs;
            }
        }
    }
}
