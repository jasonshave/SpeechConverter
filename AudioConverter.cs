using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace SpeechConverter
{
    public static class AudioConverter
    {
        internal static async Task<string> Convert(string inputFile)
        {
            SpeechRecognitionResult result;
            var speechConfig = SpeechConfig.FromSubscription("54773552be2b41149aae62b9f44876b8", "canadacentral");
            
            using(var pushStream = AudioInputStream.CreatePushStream(AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.MP3)))
            {
                using (BinaryAudioStreamReader reader = AudioHelper.CreateBinaryFileReader(inputFile))
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