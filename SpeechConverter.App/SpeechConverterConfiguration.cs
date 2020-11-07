using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeechConverter
{
    public class SpeechConverterConfiguration
    {
        private const string SubscriptionKeyArgument = "-subscriptionkey";
        private const string SubscriptionRegionArgument = "-subscriptionregion";
        private const string InputFileArgument = "-inputfile";
        private const string OutputFileArgument = "-outputfile";

        private static List<string> _oArgs;

        public string SubscriptionKey { get; }
        public string SubscriptionRegion { get; }
        public string InputFile { get; }
        public string OutputFile { get; }

        public SpeechConverterConfiguration(string[] args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            // convert to list so we can use LINQ, then convert all to lower-case
            _oArgs = args.ToList();
            _oArgs = _oArgs.ConvertAll(x => x.ToLower());

            Validate(ArgumentExists);
            Validate(ArgumentHasValue);

            SubscriptionKey = _oArgs[_oArgs.FindIndex(x => x.Contains(SubscriptionKeyArgument)) + 1];
            SubscriptionRegion = _oArgs[_oArgs.FindIndex(x => x.Contains(SubscriptionRegionArgument)) + 1];
            InputFile = _oArgs[_oArgs.FindIndex(x => x.Contains(InputFileArgument)) + 1];
            OutputFile = _oArgs[_oArgs.FindIndex(x => x.Contains(OutputFileArgument)) + 1];
        }

        private static void Validate(Action<string> predicate)
        {
            predicate(SubscriptionKeyArgument);
            predicate(SubscriptionRegionArgument);
            predicate(InputFileArgument);
            predicate(OutputFileArgument);
        }

        private static void ArgumentExists(string argument)
        {
            if(!_oArgs.Contains(argument))
                throw new ArgumentException("Could not determine parameter: {argument}", argument);
        }

        private static void ArgumentHasValue(string argumentName)
        {
            // get index of argument
            var index = _oArgs.FindIndex(x => x.Contains(argumentName));

            // verify the next argument contains a value and not another argument (-)
            if (_oArgs[index + 1].Length == 0 || _oArgs[index + 1].StartsWith("-"))
                throw new ArgumentException("There was a problem validating parameter {argumentName}", argumentName);
        }
    }
}