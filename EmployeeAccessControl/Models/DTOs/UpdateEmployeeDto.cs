using WebApplication6.Models.Enums;

namespace WebApplication6.Models.DTOs;

public class UpdateEmployeeDto
{
    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public Position? Position { get; set; }
}
