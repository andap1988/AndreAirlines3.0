using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AndreAirlinesAPI3._0Frontend.Models;

namespace AndreAirlinesAPI3._0Frontend.Data
{
    public class AndreAirlinesAPI3_0FrontendContext : DbContext
    {
        public AndreAirlinesAPI3_0FrontendContext (DbContextOptions<AndreAirlinesAPI3_0FrontendContext> options)
            : base(options)
        {
        }

        public DbSet<AndreAirlinesAPI3._0Frontend.Models.Airport> Airport { get; set; }
    }
}
