using AndreAirlinesAPI3._0Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0SearchZipcode
{
    public class SearchZipcode
    {
        public static async Task<Address> ReturnZipcode(Address addressIn)
        {
            HttpClient client = new HttpClient();
            Address address = new();
            ViaCEP viaCEP = new();

            try
            {
                HttpResponseMessage response = await client.GetAsync("http://viacep.com.br/ws/" + addressIn.ZipCode + "/json/");
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    viaCEP = JsonConvert.DeserializeObject<ViaCEP>(responseBody);

                    address.District = viaCEP.District;
                    address.City = viaCEP.City;
                    address.Country = "Brasil";
                    address.ZipCode = viaCEP.ZipCode;
                    address.Street = viaCEP.Street;
                    address.State = viaCEP.State;
                    address.Number = addressIn.Number;
                    address.Complement = viaCEP.Complement;
                    address.ErrorCode = null;

                    return address;
                }
                else
                {
                    address.ErrorCode = response.StatusCode.ToString();

                    return address;
                }

            }
            catch (HttpRequestException exception)
            {
                if (exception.StatusCode == null)
                    address.ErrorCode = exception.InnerException.Message;
                else
                    address.ErrorCode = exception.StatusCode.ToString();

                return address;
            }
        }
    }
}
