using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FakeQuotaSystem.Data;
using FakeQuotaSystem.Services;
using FakeQuotaSystem.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add DbContext
builder.Services.AddDbContext<FakeQuotaContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Add logging
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFromConfiguration();
    loggerConfiguration.ReadFromConfiguration();
    Serilog.Log.Logger = loggerConfiguration.ReadFromConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
        .Enrich.WithProperty("Machine", Environment.MachineName)
        .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] [{Properties:Application}][{Properties:Machine}][{Properties:Environment}] {Message:lj} {Exception:lj}{Properties:SourceContext}{Properties:Source}")]
        .WriteTo.File("logs/fake-quota-.log", rollingInterval: RollingInterval.Day)
        .Enrich.WithProperty("Version", Assembly.GetExecutingAssembly().GetName().Version);
});

// Add observability
var app = builder.Build();

// Enable Swagger/OpenAPI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fake Quota System API");
});

// Add Serilog request logging
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplates = new[] { "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0:000} ms" }
});

app.Run();
