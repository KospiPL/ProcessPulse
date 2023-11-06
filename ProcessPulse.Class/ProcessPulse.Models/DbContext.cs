using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions;
using ProcessPulse.Class.Service;

namespace ProcessPulse.Class.ProcessPulse.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<ProcessInfo> ProcessInfos { get; set; }
        public DbSet<TerminalMapping> TerminalMapping { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcessInfo>(entity =>
            {
                entity.Property(e => e.Id)
                      .UseIdentityColumn();

                entity.Property(e => e.Id_Process)
                      .ValueGeneratedNever();

                entity.HasOne(p => p.ParentProcess)
                      .WithMany(p => p.ChildProcesses)
                      .HasForeignKey(p => p.ParentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TerminalMapping>(entity =>
            {
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.TerminalId)
                      .IsRequired();

                entity.Property(e => e.Name)
                      .IsRequired();
            });
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            string connectionString = "Data Source=kacpercudzik.database.windows.net;Initial Catalog=ProcessPulse;Persist Security Info=True;User ID=Kacpercudzik;Password=AChiscHeDEnEl#";

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<IProcessDatabase, ProcessDatabase>();
            services.AddScoped<IProcessRepository, ProcessRepository>();
        }
    }
}