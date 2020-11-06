using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeechConverter
{
    public static class ArgumentValidation
    {
        private const string SubscriptionKeyArgument = "-subscriptionkey";
        private const string SubscriptionRegionArgument = "-subscriptionregion";
        private const string InputFileArgument = "-inputfile";
        private const string OutputFileArgument = "-outputfile";

        private static List<string> _oArgs;

        public static bool IsValidArguments(string[] args)
        {
            // convert to list so we can use LINQ, then convert all to lower-case
            _oArgs = args.ToList();
            _oArgs = _oArgs.ConvertAll(x => x.ToLower());

            // validate -subscriptionKey argument
            if(!_oArgs.Contains(SubscriptionKeyArgument))
                throw new ArgumentException(nameof(SubscriptionKeyArgument));

            // validate -subscriptionRegion argument
            if (!_oArgs.Contains(SubscriptionRegionArgument.ToLower()))
                throw new ArgumentException(nameof(SubscriptionRegionArgument));

            // validate -inputFile argument
            if (!_oArgs.Contains(InputFileArgument))
                throw new ArgumentException(nameof(InputFileArgument));

            // validate -outputFile argument
            if (!_oArgs.Contains(OutputFileArgument))
                throw new ArgumentException(nameof(OutputFileArgument));

            ValidateArgumentHasValue(SubscriptionKeyArgument);
            ValidateArgumentHasValue(SubscriptionRegionArgument);
            ValidateArgumentHasValue(InputFileArgument);
            ValidateArgumentHasValue(OutputFileArgument);

            return true;
        }

        private static void ValidateArgumentHasValue(string argumentName)
        {
            // get index of argument
            var index = _oArgs.FindIndex(x => x.Contains(argumentName));

            // verify the next argument contains a value and not another argument (-)
            if (_oArgs[index + 1].Length == 0 || _oArgs[index + 1].StartsWith("-"))
                throw new ArgumentException(nameof(argumentName));
        }
    }
}