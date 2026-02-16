using Microsoft.EntityFrameworkCore;
using WebApplication6.Data;
using WebApplication6.Models.Entities;
using WebApplication6.Services.Interfaces;

namespace WebApplication6.Services.Implementations;

public class ShiftService : IShiftService
{
    private readonly AppDbContext _context;

    public ShiftService(AppDbContext context)
    {
        _context = context;
    }

    public async Task StartShiftAsync(int employeeId, DateTime startTime)
    {
        var employee = await _context.Employees
            .Include(e => e.Shifts)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
        }

        if (employee.Shifts.Any(s => s.EndTime == null))
        {
            throw new InvalidOperationException("Employee already has an open shift. Close the previous shift first.");
        }

        var shift = new Shift
        {
            EmployeeId = employeeId,
            StartTime = startTime
        };

        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();
    }

    public async Task EndShiftAsync(int employeeId, DateTime endTime)
    {
        var employee = await _context.Employees
            .Include(e => e.Shifts)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
        }

        var openShift = employee.Shifts.FirstOrDefault(s => s.EndTime == null);
        if (openShift == null)
        {
            throw new InvalidOperationException("Employee does not have an open shift. Start a shift first.");
        }

        if (endTime < openShift.StartTime)
        {
            throw new ArgumentException("End time cannot be earlier than start time.");
        }

        openShift.EndTime = endTime;
        openShift.HoursWorked = (endTime - openShift.StartTime).TotalHours;

        await _context.SaveChangesAsync();
    }
}
