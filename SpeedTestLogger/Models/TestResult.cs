namespace SpeedTestLogger.Models
{
    public class TestResult
    {
        public string User { get; set; }
        public int Device { get; set; }
        public long Timestamp { get; set; } // ms timestamp
        public TestData Data { get; set; }
    }

    public class TestData
    {
        public TestSpeeds Speeds { get; set; }
        public TestClient Client { get; set; }
        public TestServer Server { get; set; }
    }

    public class TestSpeeds
    {
        public double Download { get; set; }
        public double Upload { get; set; }
    }

    public class TestClient
    {
        public string Ip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Isp { get; set; }
        public string Country { get; set; } // ISO 3166-1_alpha2
    }

    public class TestServer
    {
        public string Host { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Country { get; set; } // ISO 3166-1_alpha2
        public double Distance { get; set; }
        public int Ping { get; set; }
        public int Id { get; set; }
    }
}
