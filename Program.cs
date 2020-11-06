using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace SpeechConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = await Convert();
            Console.WriteLine(result);
        }

        private static async Task<string> Convert()
        {
            SpeechRecognitionResult result;
            var speechConfig = SpeechConfig.FromSubscription("54773552be2b41149aae62b9f44876b8", "canadacentral");
            
            using(var pushStream = AudioInputStream.CreatePushStream(AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.MP3)))
            {
                using (BinaryAudioStreamReader reader = Helper.CreateBinaryFileReader(@"c:\trans\audio.mp3"))
                {
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

                // Create an audio config specifying the compressed
                // audio format and the instance of your input stream class.
                var audioConfig = AudioConfig.FromStreamInput(pushStream);

                using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
                result = await recognizer.RecognizeOnceAsync();
            }

            var text = result.Text;
            return text;
        }
    }
}
