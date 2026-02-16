namespace WebApplication6.Models.Entities;

public class Shift
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public double? HoursWorked { get; set; }

    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;
}
