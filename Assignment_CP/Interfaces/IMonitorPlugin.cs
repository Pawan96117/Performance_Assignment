using Assignment_CP.Model;

namespace Assignment_CP.Interfaces
{
    public interface IMonitorPlugin
    {
        Task OnUpdateAsync(SystemMetrics metrics);
    }
}
