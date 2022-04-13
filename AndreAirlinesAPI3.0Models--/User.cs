using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Models
{
    public class User : Person
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public Function Function { get; set; }
    }
}
