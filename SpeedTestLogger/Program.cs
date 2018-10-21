using System;
using System.Threading.Tasks;
using SpeedTestLogger.Models;

namespace SpeedTestLogger
{
    class Program
    {
        static async Task Main()
        {
            var runner = new SpeedTestRunner("Norway");
            var testData = runner.RunSpeedTest();
            var results = new TestResult
            {
                User = "teodoran",
                Device = 1,
                Timestamp = 1234,
                Data = testData
            };
            var apiUrl = new Uri("http://localhost:5000");
            var client = new SpeedTestApiClient(apiUrl);
            await client.PublishTestResult(results);
        }
    }
}
