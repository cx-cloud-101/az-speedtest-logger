using Azure.Messaging.ServiceBus;
using SpeedTestLogger;
using SpeedTestLogger.Models;

Console.WriteLine("Hello SpeedTestLogger!");

var config = new LoggerConfiguration();

var serviceBusClient = new ServiceBusClient(config.ServiceBusConnectionString);
var processor = serviceBusClient.CreateProcessor("run-speedtest", "speedtest-logger-subscription",
    new ServiceBusProcessorOptions
    {
        MaxConcurrentCalls = 1,
        AutoCompleteMessages = true
    });

processor.ProcessMessageAsync += MessageHandler;

processor.ProcessErrorAsync += ErrorHandler;

await processor.StartProcessingAsync();

Console.ReadKey();


async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();

    if (body != "RUN_SPEEDTEST")
    {
        return;
    }

    Console.WriteLine($"Starting speedtest: {args.Message.SessionId}");

    var runner = new SpeedTestRunner(config.LoggerLocation);
    var testData = runner.RunSpeedTest();
    var results = new TestResult(
        SessionId: Guid.NewGuid(),
        User: config.UserId,
        Device: config.LoggerId,
        Timestamp: DateTimeOffset.Now.ToUnixTimeMilliseconds(),
        Data: testData);

    using var client = new SpeedTestApiClient(config.ApiUrl);
    var success = await client.PublishTestResult(results);

    if (success)
    {
        Console.WriteLine("Speedtest complete!");
    }
    else
    {
        Console.WriteLine("Speedtest failed!");
    }
}

// handle any errors when receiving messages
Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
    return Task.CompletedTask;
}
