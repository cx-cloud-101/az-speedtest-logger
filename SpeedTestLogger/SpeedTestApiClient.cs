using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SpeedTestLogger.Models;

namespace SpeedTestLogger
{
    public sealed class SpeedTestApiClient : IDisposable
    {
        private readonly HttpClient _client;

        public SpeedTestApiClient(Uri speedTestApiUrl)
        {
            _client = new HttpClient
            {
                BaseAddress = speedTestApiUrl
            };
        }

        public async Task<bool> PublishTestResult(TestResult result)
        {
            var json = JsonSerializer.Serialize(result);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await PostTestResult(content);
        }

        private async Task<bool> PostTestResult(StringContent result)
        {
            try
            {
                var response = await _client.PostAsync("/speedtest", result);
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Upload failed! Failure response: {0}", content);

                    return false;
                }

                return true;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Upload failed! {0}", e.Message);

                return false;
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
