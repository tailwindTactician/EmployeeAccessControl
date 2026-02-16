using Microsoft.AspNetCore.Mvc;
using WebApplication6.Models.DTOs;
using WebApplication6.Models.Enums;
using WebApplication6.Services.Interfaces;

namespace WebApplication6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HrController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public HrController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        try
        {
            var employee = await _employeeService.CreateAsync(dto);
            return Ok(employee);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
    {
        try
        {
            var employee = await _employeeService.UpdateAsync(id, dto);
            return Ok(employee);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _employeeService.DeleteAsync(id);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null)
        {
            return BadRequest($"Employee with ID {id} not found.");
        }
        return Ok(employee);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? position)
    {
        Position? positionEnum = null;
        if (!string.IsNullOrEmpty(position))
        {
            if (Enum.TryParse<Position>(position, true, out var result))
            {
                positionEnum = result;
            }
            else
            {
                return BadRequest("Invalid position specified.");
            }
        }

        var employees = await _employeeService.GetAllAsync(positionEnum);
        return Ok(employees);
    }

    [HttpGet("positions")]
    public IActionResult GetPositions()
    {
        return Ok(_employeeService.GetAllPositions());
    }

    [HttpGet("violations")]
    public async Task<IActionResult> GetViolations([FromQuery] int year, [FromQuery] int month)
    {
        if (year <= 0 || month < 1 || month > 12)
        {
            return BadRequest("Invalid year or month.");
        }

        var violations = await _employeeService.GetViolationsAsync(year, month);
        return Ok(violations);
    }
}
