using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SpeechConverter.App
{
    public static class AudioConverter
    {
        internal static async Task<string> Convert(string inputFile, string subscriptionKey, string subscriptionRegion, ILogger logger)
        {
            string result = default;

            var stopRecognition = new TaskCompletionSource<int>();

            var speechConfig = SpeechConfig.FromSubscription(subscriptionKey, subscriptionRegion);

            using (var pushStream = AudioInputStream.CreatePushStream(AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.MP3)))
            {
                using (var audioInput = AudioConfig.FromStreamInput(pushStream))
                {
                    using (var recognizer = new SpeechRecognizer(speechConfig, audioInput))
                    {
                        Console.WriteLine();

                        // subscribe to events
                        recognizer.Recognizing += (s, e) =>
                        {
                            Console.Write(".");
                        };

                        recognizer.Recognized += (s, e) =>
                        {
                            result += e.Result.Text;
                        };

                        recognizer.SessionStarted += (s, e) =>
                        {
                            logger.LogInformation("Azure Cognitive Services recognition started.");
                        };

                        recognizer.SessionStopped += (s, e) =>
                        {
                            Console.WriteLine();
                            logger.LogInformation("Azure Cognitive Services recognition stopped.");
                            stopRecognition.TrySetResult(0);
                        };

                        using (BinaryAudioStreamReader reader = AudioHelper.CreateBinaryFileReader(inputFile))
                        {
                            logger.LogInformation("Reading file {input}", inputFile);

                            // get duration of file to estimate conversion time
                            try
                            {
                                var duration = AudioHelper.GetMediaDuration(inputFile);
                                logger.LogInformation("Audio duration (seconds): {DurationOfAudio}", duration.TotalSeconds);
                                logger.LogInformation("Estimated conversion time (seconds): {ConversionEstimate}", duration.TotalSeconds / 2);
                            }
                            catch (Exception e)
                            {
                                logger.LogError("Unable to determine duration of conversion: {ExceptionMessage}", e.Message);
                            }

                            byte[] buffer = new byte[1000];
                            while (true)
                            {
                                var readSamples = reader.Read(buffer, (uint)buffer.Length);
                                if (readSamples == 0)
                                {
                                    break;
                                }
                                pushStream.Write(buffer, readSamples);
                            }
                        }
                        pushStream.Close();

                        // Starts recognition.
                        await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                        Task.WaitAny(new[] { stopRecognition.Task });

                        // Stops recognition.
                        await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                    }
                }
            }

            return result;
        }
    }
}