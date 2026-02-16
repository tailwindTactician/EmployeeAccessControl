using WebApplication6.Models.Enums;

namespace WebApplication6.Models.DTOs;

public class CreateEmployeeDto
{
    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public Position? Position { get; set; }
}
