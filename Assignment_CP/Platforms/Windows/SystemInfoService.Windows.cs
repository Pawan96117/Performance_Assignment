#if WINDOWS
using Assignment_CP.Interfaces;
using System.Management;

public class SystemInfoService : ISystemInfoService
{
    public double GetCpuUsage()
    {
        var searcher = new ManagementObjectSearcher("select LoadPercentage from Win32_Processor");
        var cpus = searcher.Get();
        double total = 0;
        int count = 0;

        foreach (var cpu in cpus)
        {
            total += Convert.ToDouble(cpu["LoadPercentage"]);
            count++;
        }

        return count > 0 ? Math.Round(total / count, 2) : 0;
    }

    public double GetMemoryUsage()
    {
        var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");

        foreach (ManagementObject obj in searcher.Get())
        {
            double totalKb = Convert.ToDouble(obj["TotalVisibleMemorySize"]);
            double freeKb = Convert.ToDouble(obj["FreePhysicalMemory"]);
            double usedKb = totalKb - freeKb;

            return Math.Round(usedKb / totalKb * 100, 2); // returns % used
        }

        return 0;
    }

    public double GetDiskUsage()
    {
        var drive = DriveInfo.GetDrives().FirstOrDefault(d => d.IsReady && d.Name == "C:\\");
        if (drive != null)
        {
            long used = drive.TotalSize - drive.AvailableFreeSpace;
            return Math.Round(100.0 * used / drive.TotalSize, 2);
        }
        return 0;
    }
    public double GetRamUsedMB()
    {
        var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");

        foreach (ManagementObject obj in searcher.Get())
        {
            double totalKb = Convert.ToDouble(obj["TotalVisibleMemorySize"]);
            double freeKb = Convert.ToDouble(obj["FreePhysicalMemory"]);
            double usedKb = totalKb - freeKb;
            return Math.Round(usedKb / 1024, 2); // Convert to MB
        }

        return 0;
    }

    public double GetDiskUsedMB()
    {
        var drive = DriveInfo.GetDrives().FirstOrDefault(d => d.IsReady && d.Name == "C:\\");
        if (drive != null)
        {
            long usedBytes = drive.TotalSize - drive.AvailableFreeSpace;
            return Math.Round(usedBytes / 1024.0 / 1024.0, 2); // Convert to MB
        }

        return 0;
    }
}
#endif