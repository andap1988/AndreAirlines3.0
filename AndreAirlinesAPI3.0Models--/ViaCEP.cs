using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Models
{
    public class ViaCEP
    {
        [JsonProperty("bairro")]
        public string District { get; set; }

        [JsonProperty("localidade")]
        public string City { get; set; }

        [JsonProperty("cep")]
        public string ZipCode { get; set; }

        [JsonProperty("logradouro")]
        public string Street { get; set; }

        [JsonProperty("uf")]
        public string State { get; set; }

        [JsonProperty("complemento")]
        public string Complement { get; set; }
    }
}
