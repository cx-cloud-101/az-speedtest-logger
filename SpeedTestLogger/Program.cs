using System;
using System.Threading.Tasks;
using SpeedTestLogger.Models;

namespace SpeedTestLogger
{
    class Program
    {   
        static async Task Main()
        {
            var config = new LoggerConfiguration();
            
            var runner = new SpeedTestRunner(config.LoggerLocation);
            var testData = runner.RunSpeedTest();
            var results = new TestResult
            {
                User = config.UserId,
                Device = config.LoggerId,
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Data = testData
            };

            var client = new SpeedTestApiClient(config.ApiUrl);
            await client.PublishTestResult(results);
        }
    }
}
