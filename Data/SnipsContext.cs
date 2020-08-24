using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Snips.Data
{
    public class SnipsContext : DbContext
    {
        public SnipsContext (DbContextOptions<SnipsContext> options)
            : base(options)
        {
        }

        public DbSet<Snips.Data.EndOfDayCheckIn> EndOfDayCheckIn { get; set; }
    }
}
