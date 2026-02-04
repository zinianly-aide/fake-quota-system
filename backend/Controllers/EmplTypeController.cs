using Microsoft.AspNetCore.Mvc;
using FakeQuotaSystem.Models;
using FakeQuotaSystem.Services;
using Serilog;

namespace FakeQuotaSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmplTypeController : ControllerBase
    {
        private readonly IEmplTypeService _emplTypeService;
        private readonly ILogger<EmplTypeController> _logger;

        public EmplTypeController(IEmplTypeService emplTypeService, ILogger<EmplTypeController> logger)
        {
            _emplTypeService = emplTypeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmplTypes()
        {
            _logger.Information("Getting all employment types");
            var types = await _emplTypeService.GetAllEmplTypesAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmplTypeById(int id)
        {
            var type = await _emplTypeService.GetEmplTypeByIdAsync(id);
            if (type == null)
            {
                _logger.Warning("Employment type with ID {Id} not found", id);
                return NotFound($"Employment type not found");
            }
            return Ok(type);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmplType([FromBody] EmplTypeCreateDto dto)
        {
            _logger.Information("Creating employment type: {Name}", dto.Name);
            var type = await _emplTypeService.CreateEmplTypeAsync(dto);
            return CreatedAtAction(type, nameof(GetEmplTypeById), id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmplType(int id, [FromBody] EmplTypeUpdateDto dto)
        {
            _logger.Information("Updating employment type: {Id}, {Name}", id, dto.Name);
            var result = await _emplTypeService.UpdateEmplTypeAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmplType(int id)
        {
            _logger.Information("Deleting employment type: {Id}", id);
            await _emplTypeService.DeleteEmplTypeAsync(id);
            return NoContent();
        }
    }
}
