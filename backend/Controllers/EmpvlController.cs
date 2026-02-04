using FakeQuotaSystem.Models;
using Microsoft.EntityFrameworkCore;
using FakeQuotaSystem.Data;
using Serilog;
using System.Text.Json;

namespace FakeQuotaSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpvlController : ControllerBase
    {
        private readonly QuotaDbContext _context;

        public EmpvlController(QuotaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<IEnumerable<Empvl>> GetAll()
        {
            var employees = _context.Empvls
                .Where(e => e.Status != "Deleted")
                .OrderByDescending(e => e.UpdateDate)
                .Take(100)
                .ToList();

            Log.Information("Retrieved {Count} employees", employees.Count);

            return Ok(new
            {
                status = "success",
                count = employees.Count,
                data = employees
            });
        }

        [HttpGet]
        [Route("active")]
        public ActionResult<IEnumerable<Empvl>> GetActiveEmployees()
        {
            var employees = _context.Empvls
                .Where(e => e.Status == "Active" && e.Certificate != null)
                .OrderBy(e => e.EmpId)
                .ToList();

            Log.Information("Retrieved {Count} active employees", employees.Count);

            return Ok(new
            {
                status = "success",
                count = employees.Count,
                data = employees
            });
        }

        [HttpPost]
        [Route("create")]
        public ActionResult<Empvl> Create([FromBody] Empvl employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                employee.Id = _context.Empvls.Count() + 1;
                employee.Status = "Active";
                employee.CreateDate = DateTime.UtcNow;
                employee.UpdateDate = null;

                _context.Empvls.Add(employee);
                _context.SaveChanges();

                Log.Information("Created employee: {EmpId} - {ActivityName}", 
                    employee.EmpId, employee.ActivityName);

                return CreatedAtAction("/", employee);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create employee");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut]
        [Route("{id:long}/update")]
        public ActionResult<Empvl> Update(long id, [FromBody] Empvl employee)
        {
            var existing = _context.Empvls.FirstOrDefault(e => e.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            try
            {
                existing.ActivityName = employee.ActivityName;
                existing.ActivityDay = employee.ActivityDay;
                existing.Certificate = employee.Certificate;
                existing.UpdateEmpId = employee.UpdateEmpId;
                existing.UpdateEmpName = employee.UpdateEmpName;
                existing.UpdateDate = DateTime.UtcNow;
                existing.Status = employee.Status;
                existing.Rdptaskid = employee.Rdptaskid;
                existing.Rdppnodeaccount = employee.Rdppnodeaccount;
                existing.Rdppnodenameber = employee.Rdppnodenameber;

                _context.Empvls.Update(existing);
                _context.SaveChanges();

                Log.Information("Updated employee: {EmpId}", existing.Id);

                return Ok(new
                {
                    status = "success",
                    data = existing
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to update employee");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{id:long}")]
        public ActionResult Delete(long id)
        {
            var existing = _context.Empvls.FirstOrDefault(e => e.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            try
            {
                existing.Status = "Deleted";
                existing.UpdateEmpId = $"DELETED-{existing.Id}";
                existing.UpdateDate = DateTime.UtcNow;

                _context.Empvls.Update(existing);
                _context.SaveChanges();

                Log.Information("Deleted employee: {EmpId}", existing.Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to delete employee");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
