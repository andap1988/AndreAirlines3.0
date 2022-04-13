using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Ticket.Service
{
    public class SearchUser
    {
        public static async Task<User> ReturnUser(string loginUser, string token)
        {
            HttpClient client = new HttpClient();
            User user = new();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44345/api/Users/loginUser/" + loginUser);
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

        public static async Task<User> ReturnUserLogin(User userIn)
        {
            HttpClient client = new HttpClient();
            User user = new();


            try
            {
                var json = JsonConvert.SerializeObject(userIn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://localhost:44345/api/Users/userlogin/", content);
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
