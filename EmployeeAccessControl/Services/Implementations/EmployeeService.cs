using Microsoft.EntityFrameworkCore;
using WebApplication6.Data;
using WebApplication6.Models.DTOs;
using WebApplication6.Models.Entities;
using WebApplication6.Models.Enums;
using WebApplication6.Services.Interfaces;

namespace WebApplication6.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Employee> CreateAsync(CreateEmployeeDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName) || 
            string.IsNullOrWhiteSpace(dto.LastName) ||
            !dto.Position.HasValue)
        {
            throw new ArgumentException("First name, Last name and Position are mandatory.");
        }

        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            Position = dto.Position.Value
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task<Employee> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found.");
        }

        if (!string.IsNullOrWhiteSpace(dto.FirstName)) employee.FirstName = dto.FirstName;
        if (!string.IsNullOrWhiteSpace(dto.LastName)) employee.LastName = dto.LastName;
        if (dto.MiddleName != null) employee.MiddleName = dto.MiddleName;
        if (dto.Position.HasValue) employee.Position = dto.Position.Value;

        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found.");
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<object?> GetByIdAsync(int id)
    {
        var e = await _context.Employees
            .Include(e => e.Shifts)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (e == null) return null;

        var now = DateTime.Now;
        var violations = CalculateViolations(e, now.Year, now.Month);

        return new
        {
            e.Id,
            e.FirstName,
            e.LastName,
            e.MiddleName,
            Position = e.Position.ToString(),
            CurrentMonthViolations = violations
        };
    }

    public async Task<IEnumerable<object>> GetAllAsync(Position? position)
    {
        var query = _context.Employees
            .Include(e => e.Shifts)
            .AsQueryable();

        if (position.HasValue)
        {
            query = query.Where(e => e.Position == position.Value);
        }

        var employees = await query.ToListAsync();
        var now = DateTime.Now;

        return employees.Select(e =>
        {
            var violations = CalculateViolations(e, now.Year, now.Month);
            return new
            {
                e.Id,
                e.FirstName,
                e.LastName,
                e.MiddleName,
                Position = e.Position.ToString(),
                CurrentMonthViolations = violations
            };
        });
    }

    private int CalculateViolations(Employee e, int year, int month)
    {
        int violations = 0;
        var monthShifts = e.Shifts.Where(s => s.StartTime.Year == year && s.StartTime.Month == month);

        foreach (var shift in monthShifts)
        {
            if (e.Position == Position.CandleTester)
            {
                if (shift.EndTime.HasValue && shift.EndTime.Value.Hour < 21)
                {
                    violations++;
                }
            }
            else
            {
                bool lateComing = shift.StartTime.TimeOfDay > new TimeSpan(9, 0, 0);
                bool earlyLeaving = shift.EndTime.HasValue && shift.EndTime.Value.TimeOfDay < new TimeSpan(18, 0, 0);

                if (lateComing || earlyLeaving)
                {
                    violations++;
                }
            }
        }
        return violations;
    }

    public IEnumerable<string> GetAllPositions()
    {
        return Enum.GetNames(typeof(Position));
    }

    public async Task<IEnumerable<object>> GetViolationsAsync(int year, int month)
    {
        var employees = await _context.Employees
            .Include(e => e.Shifts)
            .ToListAsync();

        var result = employees.Select(e =>
        {
            int violations = CalculateViolations(e, year, month);

            return new
            {
                EmployeeId = e.Id,
                FullName = $"{e.LastName} {e.FirstName} {e.MiddleName}".Trim(),
                Position = e.Position.ToString(),
                ViolationsCount = violations
            };
        });

        return result;
    }
}
