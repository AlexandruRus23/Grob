using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CpuUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            while (true)
            {
                Console.WriteLine($"CPU Usage: {cpuCounter.NextValue()}%");
                Thread.Sleep(1000);
            }
        }
    }
}
