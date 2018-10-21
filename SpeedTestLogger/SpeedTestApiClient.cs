using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpeedTestLogger.Models;

namespace SpeedTestLogger
{
    public class SpeedTestApiClient : IDisposable
    {
        private readonly HttpClient _client;

        public SpeedTestApiClient(Uri speedTestApiUrl)
        {
            _client = new HttpClient
            {
                BaseAddress = speedTestApiUrl
            };
        }

        public async Task PublishTestResult(TestResult result)
        {
            var json = JsonConvert.SerializeObject(result);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await PostTestResult(content);
        }

        private async Task PostTestResult(StringContent result)
        {
            try
            {
                var response = await _client.PostAsync("/SpeedTest", result);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
