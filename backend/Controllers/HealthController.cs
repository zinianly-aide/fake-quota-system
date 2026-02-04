using Microsoft.AspNetCore.Mvc;
using FakeQuotaSystem.Models;
using System.Text.Json;

namespace FakeQuotaSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route("health")]
        public IActionResult GetHealth()
        {
            var response = new
            {
                status = "Healthy",
                version = "1.0.0",
                timestamp = DateTime.UtcNow,
                system = "Fake Quota Management System",
                database = "Oracle"
                frontend = "Blazor WebAssembly",
                features = new
                {
                    regions = 4,
                    empvl_types = 4,
                    total_applications = 100
                    active_applications = 85,
                    pending_approvals = 15
                }
            };

            return Ok(response);
        }
    }
}
