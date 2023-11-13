using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProcessPulse.Class.ProcessPulse.Models;

namespace ProcessPulse.ServerService.ProcessPulse.Dbcontext
{
    public class FlotaDbContext : DbContext
    {
        public FlotaDbContext(DbContextOptions<FlotaDbContext> options) : base(options) { }

        public DbSet<FlotaModel> FlotaModels { get; set; }
    }
}

