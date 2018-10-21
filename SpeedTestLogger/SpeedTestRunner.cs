using System;
using System.Linq;
using SpeedTest;
using SpeedTest.Models;
using SpeedTestLogger.Models;

namespace SpeedTestLogger
{
    public class SpeedTestRunner
    {
        private readonly SpeedTestClient _client;
        private readonly Settings _settings;
        private readonly string _country;

        public SpeedTestRunner(string country)
        {
            _client = new SpeedTestClient();
            _settings = _client.GetSettings();
            _country = country;
        }

        public TestData RunSpeedTest()
        {
            var server = FindBestTestServer();

            var downloadSpeed = TestDownloadSpeed(server);
            var uploadSpeed = TestUploadSpeed(server);

            return new TestData
            {
                Speeds = new TestSpeeds
                {
                    Download = downloadSpeed,
                    Upload = uploadSpeed
                },
                Client = new TestClient
                {
                    Ip = _settings.Client.Ip,
                    Latitude = _settings.Client.Latitude,
                    Longitude = _settings.Client.Longitude,
                    Isp = _settings.Client.Isp,
                    Country = _country
                },
                Server = new TestServer
                {
                    Host = server.Host,
                    Latitude = server.Latitude,
                    Longitude = server.Longitude,
                    Country = server.Country,
                    Distance = server.Distance,
                    Ping = server.Latency,
                    Id = server.Id
                }
            };
        }

        private Server FindBestTestServer()
        {
            return _settings.Servers
                .Where(s => s.Country.Equals(_country))
                .Take(10)
                .Select(s =>
                {
                    s.Latency = _client.TestServerLatency(s);
                    return s;
                })
                .OrderBy(s => s.Latency)
                .First();
        }

        private double TestDownloadSpeed(Server server)
        {
            var downloadSpeed = _client.TestDownloadSpeed(server, _settings.Download.ThreadsPerUrl);
            
            return ConvertSpeedToMbps(downloadSpeed);
        }

        private double TestUploadSpeed(Server server)
        {
            var uploadSpeed = _client.TestUploadSpeed(server, _settings.Upload.ThreadsPerUrl);

            return ConvertSpeedToMbps(uploadSpeed);
        }

        private double ConvertSpeedToMbps(double speed)
        {
            return Math.Round(speed / 1024, 2);
        }
    }
}
