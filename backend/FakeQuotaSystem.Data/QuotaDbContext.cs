using Microsoft.EntityFrameworkCore;
using FakeQuotaSystem.Models;
using Serilog;
using System.Reflection;

namespace FakeQuotaSystem.Models
{
    public class QuotaDbContext : DbContext
    {
        public QuotaDbContext(DbContextOptions<QuotaDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmplQuota> EmplQuotas { get; set; }
        public DbSet<Empvl> Empvls { get; set; }
    }

    public static class QuotaDbContextOptionsFactory
    {
        public static QuotaDbContextOptions Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuotaDbContext>();
            
            optionsBuilder.UseOracle(
                connectionString,
                options =>
                {
                    // Enable detailed error logging
                    options.EnableDetailedErrors(true);
                    
                    // Set command timeout to 300 seconds (5 minutes)
                    options.SetCommandTimeout(300);
                    
                    // Enable sensitive data logging
                    options.EnableSensitiveDataLogging(true);
                    
                    // Set default fetch size to 100
                    options.DefaultCommandFetchSize(100);
                });
                
            return optionsBuilder.Options;
        }
    }
}
