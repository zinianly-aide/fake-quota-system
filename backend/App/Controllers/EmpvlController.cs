using Microsoft.AspNetCore.Mvc;

namespace FakeQuotaSystem.App.Controllers;

[ApiController]
[Route("api/empvl")]
public class EmpvlController : ControllerBase
{
    private readonly FakeDataStore _store;

    public EmpvlController(FakeDataStore store)
    {
        _store = store;
    }

    [HttpGet("all")]
    public IActionResult GetAll() => Ok(_store.GetEmployees());

    [HttpGet("active")]
    public IActionResult GetActive() => Ok(_store.GetActiveEmployees());

    [HttpPost("create")]
    public IActionResult Create([FromBody] Empvl employee)
    {
        var created = _store.AddEmployee(employee);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpPut("{id:long}/update")]
    public IActionResult Update(long id, [FromBody] Empvl employee)
    {
        return _store.UpdateEmployee(id, employee) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:long}")]
    public IActionResult Delete(long id)
    {
        return _store.DeleteEmployee(id) ? NoContent() : NotFound();
    }
}
