using System;
using System.Collections.Generic;

#nullable disable

namespace AndreAirlinesAPI3._0AiportDataEF.Model
{
    public partial class Airport
    {
        public long Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string Continent { get; set; }
    }
}
