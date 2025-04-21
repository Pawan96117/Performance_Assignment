using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Assignment_CP.Model;
using System.Text;
using System.Text.Json;
using Assignment_CP.Interfaces;

public class ApiPostPlugin : IMonitorPlugin
{
    private readonly HttpClient _httpClient = new();
    private readonly IConfiguration _config;

    public ApiPostPlugin(IConfiguration config)
    {
        _config = config;
    }

    public async Task OnUpdateAsync(SystemMetrics metrics)
    {
        var payload = new
        {
            cpu = metrics.Cpu,
            ram_used = metrics.RamUsedMB,
            disk_used = metrics.DiskUsedMB
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var url = _config["Monitoring:ApiUrl"];

        if (!string.IsNullOrWhiteSpace(url))
        {
            try
            {
                var response = await _httpClient.PostAsync(url, content);
                Debug.WriteLine($"[API] Posted to {url}, Status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[API] ERROR: {ex.Message}");
            }
        }
    }
}
