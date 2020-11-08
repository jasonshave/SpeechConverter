using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace SpeechConverter
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        private static string[] _args;

        static async Task Main(string[] args)
        {
            try
            {
                // todo: remove this part
                //_args =
                //    @"-subscriptionkey 54773552be2b41149aae62b9f44876b8 -subscriptionregion canadacentral -inputfile C:\trans\Jackson_D_2019_02_19_02.MP3 -outputFile C:\trans\Jackson_D_2019_02_19_02.txt".Split(" ").ToArray();
                _args = args;
                await StartApplication();
            }
            catch (ArgumentException)
            {
                HelpPage.ShowHelp();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.Message);
            }

        }

        private static async Task StartApplication()
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

            var host = CreateHostBuilder().Build();
            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;

            var application = services.GetRequiredService<Application>();
            await application.Run(_args);
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
            Host.CreateDefaultBuilder()
                .ConfigureServices((services) =>
                {
                    services.AddSingleton<Application>();
                    services.AddSingleton<SpeechConverterConfiguration>();
                })
                .UseSerilog();
    }
}
