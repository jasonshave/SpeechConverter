using System;

namespace SpeechConverter.App
{
    public static class HelpPage
    {
        public static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: SpeechConverter.exe -subscriptionKey <key> -subscriptionRegion <region> -inputFile <inputfile> -outputFile <outputfile>");
            Console.WriteLine();
            Console.WriteLine("Execute an Azure Cognitive Services audio transcription to a .txt file.");
            Console.WriteLine();
            Console.WriteLine("Optional parameters:");
            Console.WriteLine();
            Console.WriteLine(" -help | -h              Show command line help.");
            Console.WriteLine("Required parameters:");
            Console.WriteLine(" -subscriptionKey        The subscription key in Azure Cognitive Services.");
            Console.WriteLine(" -subscriptionRegion     The region hosting your Azure Cognitive Services instance.");
            Console.WriteLine(" -inputFile              The MP3 file you want to transcribe into text.");
            Console.WriteLine(" -outputFile             The text file you want the transcribed voice written to. If the file exists, it will be overwritten.");
            Console.WriteLine();
        }
    }
}