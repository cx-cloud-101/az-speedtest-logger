﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using SpeedTestLogger.Models;

namespace SpeedTestLogger
{
    class Program
    {
        private static LoggerConfiguration _config;
        private static SubscriptionClient _subscriptionClient;

        static async Task Main()
        {
            Console.WriteLine("Starting SpeedTestLogger");
            Console.WriteLine("Press any key to exit");

            _config = new LoggerConfiguration();
            var options = new MessageHandlerOptions(HandleException)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = true
            };
            _subscriptionClient = new SubscriptionClient(_config.ServiceBus.ConnectionString, _config.ServiceBus.TopicName, _config.ServiceBus.SubscriptionName);
            _subscriptionClient.RegisterMessageHandler(HandleSpeedTestMessage, options);

            Console.ReadKey();

            await _subscriptionClient.CloseAsync();
        }

        static async Task HandleSpeedTestMessage(Message message, CancellationToken token)
        {
            var messageBody = Encoding.UTF8.GetString(message.Body);
            if (messageBody != "RUN_SPEEDTEST")
            {
                return;
            }

            Console.WriteLine($"Starting speedtest: { message.SessionId }");

            var runner = new SpeedTestRunner(_config.LoggerLocation);
            var testData = runner.RunSpeedTest();

            Console.WriteLine("Got download: {0} Mbps and upload: {1} Mbps", testData.Speeds.Download, testData.Speeds.Upload);
            var results = new TestResult
            {
                SessionId = Guid.Parse(message.SessionId),
                User = _config.UserId,
                Device = _config.LoggerId,
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Data = testData
            };

            Console.WriteLine("Uploading data to speedtest API");
            var success = false;
            using (var client = new SpeedTestApiClient(_config.ApiUrl))
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

        static Task HandleException(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");

            return Task.CompletedTask;
        }
    }
}
