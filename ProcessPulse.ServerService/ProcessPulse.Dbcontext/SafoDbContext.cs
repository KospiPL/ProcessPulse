using Microsoft.EntityFrameworkCore;
using ProcessPulse.Class.ProcessPulse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPulse.ServerService.ProcessPulse.Dbcontext
{
    public class SafoDbContext : DbContext
    {
        public SafoDbContext(DbContextOptions<SafoDbContext> options) : base(options) { }

        public DbSet<SafoModel> SafoModels { get; set; }
    }
}
