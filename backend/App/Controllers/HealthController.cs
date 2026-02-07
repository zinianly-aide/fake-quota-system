using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace FakeQuotaSystem.App.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public HealthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var mySqlConnection = _configuration.GetConnectionString("MySqlConnection");
        var databaseStatus = "unreachable";
        string? databaseError = null;

        if (!string.IsNullOrWhiteSpace(mySqlConnection))
        {
            try
            {
                await using var connection = new MySqlConnection(mySqlConnection);
                await connection.OpenAsync(cancellationToken);
                databaseStatus = "ok";
            }
            catch (Exception ex)
            {
                databaseError = ex.Message;
            }
        }

        return Ok(new
        {
            status = "Healthy",
            version = "1.0.0",
            timestamp = DateTime.UtcNow,
            system = "Fake Quota Management System",
            database = databaseStatus,
            databaseError
        });
    }
}
