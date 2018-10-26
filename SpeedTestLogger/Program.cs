using System;
using System.Threading.Tasks;
using SpeedTestLogger.Models;

namespace SpeedTestLogger
{
    class Program
    {   
        static async Task Main()
        {
            Console.WriteLine("Starting SpeedTestLogger");
            var config = new LoggerConfiguration();

            Console.WriteLine("Running new speedtest");
            var runner = new SpeedTestRunner(config.LoggerLocation);
            var testData = runner.RunSpeedTest();

            Console.WriteLine("Got download: {0} Mbps and upload: {1} Mbps", testData.Speeds.Download, testData.Speeds.Upload);
            var results = new TestResult
            {
                User = config.UserId,
                Device = config.LoggerId,
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Data = testData
            };

            Console.WriteLine("Uploading data to speedtest API");
            var success = false;
            using (var client = new SpeedTestApiClient(config.ApiUrl))
            {
                success = await client.PublishTestResult(results);    
            }
            
            if (success)
            {
                Console.WriteLine("Speedtest complete!");
            }
            else {
                Console.WriteLine("Speedtest failed!");
            }
        }
    }
}
