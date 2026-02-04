using Microsoft.EntityFrameworkCore;
using FakeQuotaSystem.Services;
using FakeQuotaSystem.Data;
using FakeQuotaSystem.Models;
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
                    .AddJson())
                    .AddConsole())
                    .CreateLogger());
            
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                
                // Add services to DI container
                builder.Services.AddDbContext<QuotaDbContext>(options =>
                    options.UseOracle(
                        GetConnectionString("OracleConnection"),
                        opt =>
                        {
                            opt.EnableDetailedErrors(true);
                            opt.SetCommandTimeout(300);
                            opt.EnableSensitiveDataLogging(true);
                            opt.DefaultCommandFetchSize(100);
                        }
                    ));
                
                builder.Services.AddScoped<IApplicationService, ApplicationService>();
                builder.Services.AddControllers();
                
                // Add Swagger/OpenAPI
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("Fake Quota Management System API v1.0");
                    c.IncludeXmlComments();
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme",
                        Name = "Bearer",
                        In = "header",
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Type = SecuritySchemeType.ApiKey
                    });
                    c.AddSecurityRequirement("Bearer");
                });
                
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("Fake Quota Management System API v1.0");
                    c.IncludeXmlComments();
                });
                
                // Add CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
                });
                
                // Configure the HTTP request pipeline
                builder.Services.AddControllers();
                
                var app = builder.Build();
                
                // Configure middleware
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHttpsRedirection();
                app.UseCors("AllowAll");
                app.UseAuthorization();
                
                app.MapControllers();
                
                // Seed data if needed
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<QuotaDbContext>();
                    
                    // Ensure tables exist
                    context.Database.EnsureCreated();
                    
                    Log.Information("==========================================");
                    Log.Information("Fake Quota Management System Started");
                    Log.Information("==========================================");
                    Log.Information($"Environment: {GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production")}");
                    Log.Information($"Framework: .NET {GetAssemblyVersion(typeof(Program)).GetName())}");
                    Log.Information($"Database: Oracle");
                    Log.Information($"API Endpoints: http://localhost:8080");
                    Log.Information($"Swagger UI: http://localhost:8080/swagger");
                    Log.Information("==========================================");
                }
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
