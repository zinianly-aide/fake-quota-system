using Microsoft.EntityFrameworkCore;
using FakeQuotaSystem.Models;
using FakeQuotaSystem.Data;
using Serilog;
using System.Reflection;

namespace FakeQuotaSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .AddJson()
                    .AddConsole())
                    .CreateLogger());
            
            try
            {
                Log.Information("Starting Fake Quota Management System...");
                Log.Information("==========================================");
                Log.Information("System Configuration");
                Log.Information("==========================================");
                Log.Information($"Environment: {GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production")}");
                Log.Information($"Oracle Connection: {GetConnectionString("OracleConnection")?.Substring(0, 20)}***");
                Log.Information($"Framework: .NET {GetAssemblyVersion(typeof(Program).GetName())}");
                Log.Information("==========================================");
                
                // Create DbContext
                var context = new QuotaDbContext(QuotaDbContextOptionsFactory.Create(
                    GetConnectionString("OracleConnection")
                ));
                
                // TODO: Add services to DI container
                // TODO: Configure Swagger
                // TODO: Configure CORS
                
                Log.Information("System initialized successfully");
                Log.Information("==========================================");
                Log.Information("");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to start system");
                Log.Fatal($"Error: {ex.Message}");
                Log.Fatal($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}
