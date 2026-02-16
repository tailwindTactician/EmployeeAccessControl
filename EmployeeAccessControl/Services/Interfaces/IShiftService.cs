namespace WebApplication6.Services.Interfaces;

public interface IShiftService
{
    Task StartShiftAsync(int employeeId, DateTime startTime);
    Task EndShiftAsync(int employeeId, DateTime endTime);
}
