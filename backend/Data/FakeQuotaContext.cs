using Microsoft.EntityFrameworkCore;
using FakeQuotaSystem.Models;

namespace FakeQuotaSystem.Data
{
    public class FakeQuotaContext : DbContext
    {
        public FakeQuotaContext(DbContextOptions<FakeQuotaContext> options)
            : base(options)
        {
        }

        public DbSet<EmplQuota> EmplQuotas { get; set; }
        public DbSet<Empl> Emps { get; set; }
        public DbSet<RdppTask> RdppTasks { get; set; }
        public DbSet<RdppNodeAccount> RdppNodeAccounts { get; set; }
        public DbSet<RdppViewer> RdppViewers { get; set; }
    }
}
