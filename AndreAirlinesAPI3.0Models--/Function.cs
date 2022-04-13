using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Models
{
    public class Function
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<Access> Access { get; set; }
    }
}
