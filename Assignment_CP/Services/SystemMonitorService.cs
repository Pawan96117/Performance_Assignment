using Assignment_CP.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_CP.Services
{
    public class SystemMonitorService
    {
        private readonly ISystemInfoService _systemInfoService;
        private readonly IConfiguration _config;
        private readonly IEnumerable<IMonitorPlugin> _plugins;
        private Timer? _timer;

        public SystemMonitorService(
            ISystemInfoService systemInfoService,
            IConfiguration config,
            IEnumerable<IMonitorPlugin> plugins)
        {
            _systemInfoService = systemInfoService;
            _config = config;
            _plugins = plugins;
        }

        public void Start()
        {
            int interval = int.Parse(_config["Monitoring:IntervalSeconds"] ?? "5");
            Debug.WriteLine($"[Monitor] Starting monitor (interval: {interval}s)");
            _timer = new Timer(OnTimerTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(interval));
        }

        private async void OnTimerTick(object? state)
        {
            try
            {
                var metrics = new Model.SystemMetrics
                {
                    Cpu = _systemInfoService.GetCpuUsage(),
                    RamUsedMB = _systemInfoService.GetRamUsedMB(),
                    DiskUsedMB = _systemInfoService.GetDiskUsedMB()
                };

                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] CPU: {metrics.Cpu}%, RAM: {metrics.RamUsedMB} MB, Disk: {metrics.DiskUsedMB} MB");

                foreach (var plugin in _plugins)
                {
                    await plugin.OnUpdateAsync(metrics);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Monitor] ERROR: {ex.Message}");
            }
        }
    }

}
