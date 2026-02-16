using WebApplication6.Models.Entities;
using WebApplication6.Models.DTOs;
using WebApplication6.Models.Enums;

namespace WebApplication6.Services.Interfaces;

public interface IEmployeeService
{
    Task<Employee> CreateAsync(CreateEmployeeDto dto);
    Task<Employee> UpdateAsync(int id, UpdateEmployeeDto dto);
    Task DeleteAsync(int id);
    Task<object?> GetByIdAsync(int id);
    Task<IEnumerable<object>> GetAllAsync(Position? position);
    IEnumerable<string> GetAllPositions();
    
    
    Task<IEnumerable<object>> GetViolationsAsync(int year, int month);
}