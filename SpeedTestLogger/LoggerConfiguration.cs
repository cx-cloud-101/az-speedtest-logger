using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace SpeedTestLogger;

public class LoggerConfiguration
{
    public readonly string UserId;
    public readonly int LoggerId;
    public readonly RegionInfo LoggerLocation;
    public readonly Uri ApiUrl;
    public readonly string ServiceBusConnectionString;

    public LoggerConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        var configuration = builder.Build();

        UserId = configuration["userId"];
        LoggerId = int.Parse(configuration["loggerId"]);
        LoggerLocation = new RegionInfo(configuration["loggerLocationCountryCode"]);
        ApiUrl = new Uri(configuration["speedTestApiUrl"]);
        ServiceBusConnectionString = configuration["serviceBusConnectionString"];

        Console.WriteLine("Logger located in {0}", LoggerLocation.EnglishName);
    }
}
