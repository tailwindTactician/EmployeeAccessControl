using WebApplication6.Models.Enums;

namespace WebApplication6.Models.Entities;

public class Employee
{
    public int Id { get; set; }

    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public Position Position { get; set; }

    public List<Shift> Shifts { get; set; } = new();
}
