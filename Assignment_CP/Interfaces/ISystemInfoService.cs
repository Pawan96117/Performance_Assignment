using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_CP.Interfaces
{
    public interface ISystemInfoService
    {
        double GetCpuUsage();    
        double GetMemoryUsage(); 
        double GetDiskUsage();   
        double GetRamUsedMB();  
        double GetDiskUsedMB(); 
    }
}
