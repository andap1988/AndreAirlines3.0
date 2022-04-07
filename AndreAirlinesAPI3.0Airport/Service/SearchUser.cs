using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Airport.Service
{
    public class SearchUser
    {
        public static async Task<User> ReturnUser(string id)
        {
            HttpClient client = new HttpClient();
            User user = new();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44345/api/Users/" + id);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(responseBody);
                    user.ErrorCode = null;

                    return user;
                }
                else
                {
                    user.ErrorCode = response.StatusCode.ToString();

                    return user;
                }

            }
            catch (HttpRequestException exception)
            {
                if (exception.StatusCode == null)
                    user.ErrorCode = exception.InnerException.Message;
                else
                    user.ErrorCode = exception.StatusCode.ToString();

                return user;
            }
        }
    }
}
