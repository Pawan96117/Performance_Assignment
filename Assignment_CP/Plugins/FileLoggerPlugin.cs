using System.Text.Json;
using Assignment_CP.Interfaces;
using Assignment_CP.Model;

public class FileLoggerPlugin : IMonitorPlugin
{
    private readonly string _logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SystemMetricsLog.txt");

    public async Task OnUpdateAsync(SystemMetrics metrics)
    {
        var logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - CPU: {metrics.Cpu}%, RAM: {metrics.RamUsedMB} MB, Disk: {metrics.DiskUsedMB} MB";
        await File.AppendAllTextAsync(_logPath, logLine + Environment.NewLine);
    }
}