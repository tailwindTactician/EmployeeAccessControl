using Microsoft.AspNetCore.Mvc;
using WebApplication6.Services.Interfaces;

namespace WebApplication6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckpointController : ControllerBase
{
    private readonly IShiftService _shiftService;

    public CheckpointController(IShiftService shiftService)
    {
        _shiftService = shiftService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartShift([FromQuery] int employeeId, [FromQuery] DateTime? time)
    {
        try
        {
            var startTime = time ?? DateTime.Now;
            await _shiftService.StartShiftAsync(employeeId, startTime);
            return Ok($"Shift started for employee {employeeId} at {startTime}");
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("end")]
    public async Task<IActionResult> EndShift([FromQuery] int employeeId, [FromQuery] DateTime? time)
    {
        try
        {
            var endTime = time ?? DateTime.Now;
            await _shiftService.EndShiftAsync(employeeId, endTime);
            return Ok($"Shift ended for employee {employeeId} at {endTime}");
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
