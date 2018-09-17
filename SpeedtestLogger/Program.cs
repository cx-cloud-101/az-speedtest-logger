using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SpeedTest;
using SpeedTest.Models;

namespace SpeedtestLogger
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Getting speedtest.net settings and server list...");
            var client = new SpeedTestClient();
            var settings = client.GetSettings();

            Console.WriteLine("Finding best server...");
            var bestServer = settings.Servers
                .Where(s => s.Country.Equals("Norway"))
                .Take(10)
                .Select(s =>
                {
                    s.Latency = client.TestServerLatency(s);
                    return s;
                })
                .OrderBy(s => s.Latency)
                .First();

            Console.WriteLine("Testing speed...");
            
            var downloadSpeed = client.TestDownloadSpeed(bestServer, settings.Download.ThreadsPerUrl);
            Console.WriteLine("Download speed: {0} Mbps", Math.Round(downloadSpeed / 1024, 2));

            var uploadSpeed = client.TestUploadSpeed(bestServer, settings.Upload.ThreadsPerUrl);
            Console.WriteLine("Upload speed: {0} Mbps", Math.Round(uploadSpeed / 1024, 2));

            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();
        }
    }
}
