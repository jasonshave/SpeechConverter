using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SpeechConverter.App
{
    public class Application
    {
        private readonly ILogger _logger;
        private readonly SpeechConverterConfiguration _speechConverterConfiguration;

        public Application(ILogger<Application> logger, SpeechConverterConfiguration speechConverterConfiguration)
        {
            _logger = logger;
            _speechConverterConfiguration = speechConverterConfiguration;
        }

        public async Task Run(string[] args)
        {
            _speechConverterConfiguration.Initialize(args);

            var result = await AudioConverter.Convert(
                inputFile: _speechConverterConfiguration.InputFile,
                subscriptionKey: _speechConverterConfiguration.SubscriptionKey,
                subscriptionRegion: _speechConverterConfiguration.SubscriptionRegion,
                _logger);

            _logger.LogInformation("Writing {Characters} characters to {FileName}.", result.Length, _speechConverterConfiguration.OutputFile);

            await File.WriteAllTextAsync(_speechConverterConfiguration.OutputFile, result);
        }
    }
}