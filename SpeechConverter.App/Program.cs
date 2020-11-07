using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Gst;

namespace SpeechConverter
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            SetupLogging();
            Log.Logger.Information("Application starting.");

            try
            {
                var speechConverterConfiguration = new SpeechConverterConfiguration(args);
                var result = await AudioConverter.Convert(inputFile: speechConverterConfiguration.InputFile);
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.Message);
            }

            Log.Logger.Information("Application exiting.");
        }

        private static void SetupLogging()
        {
            // Set up configuration builder for Serilog
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            CreateHostBuilder().Build();
        }

        private static void BuildConfig(IConfigurationBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder().UseSerilog();
    }
}
