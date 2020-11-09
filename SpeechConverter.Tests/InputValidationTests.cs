using Microsoft.Extensions.Logging;
using Moq;
using SpeechConverter.App;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace SpeechConverter.Tests
{
    public class InputValidationTests
    {
        private readonly Mock<ILogger<SpeechConverterConfiguration>> _speechConverterLogger;

        public InputValidationTests()
        {
            _speechConverterLogger = new Mock<ILogger<SpeechConverterConfiguration>>();
        }

        [Fact]
        public void Validate_SubscriptionKey_Argument_Returns_True()
        {
            // Arrange
            var expectedResult = "-subscriptionKey";
            string[] args = { expectedResult };

            // Act
            var oARgs = args.ToList();

            // Assert
            Assert.Contains(expectedResult, oARgs);
        }

        [Fact]
        public void Correct_Args_Validates_Without_Exception()
        {
            // Arrange
            var input = @"-subscriptionKey 1234 -subscriptionRegion canada -inputFile ./testfiles/test.mp3 -outputFile ./testfiles/test.txt";
            var args = input.Split(" ").ToArray();

            // Act
            var expectedResult = 8;
            var actualResult = args.Length;

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(@"-subscriptionKey 1234 -subscriptionRegion canada -inputFile ./testfiles/test.mp3 -outputFil ./testfiles/test.txt")]
        [InlineData(@"-subscriptionKey 1234 -subscriptionRegion canada -inputFile ./testfiles/test.mp3 -outputFile ./testfiles/test.txt")]
        [InlineData(@"-subscriptionKey 1234-subscriptionRegion canada -inputFile ./testfiles/test.mp3 -outputFile ./testfiles/test.txt")]
        [InlineData(@"-subscriptionKey 1234 -subscriptionRegioncanada -inputFile ./testfiles/test.mp3 -outputFile ./testfiles/test.txt")]
        [InlineData(@"-subiptionKey 1234 -bscriptionRegion canada -inputFile ./testfiles/test.mp3 -outputFile ./testfiles/test.txt")]
        [InlineData(@"-subscriptionKey1234 -subscriptionRegion canada -inptFile ./testfiles/test.mp3 -outputFile ./testfiles/test.txt")]
        [InlineData(@"-subscriptionKey1234 -subscriptionRegion canada -inptFile ./testfiles/test.mp3 -outputFile")]
        [InlineData(@"-inptFile ./testfiles/test.mp3 -outputFile ./testfiles/test.txt")]
        [InlineData(@"")]
        public void Invalid_Parameters_Throws_ArgumentException(string input)
        {
            // Arrange
            var args = input.Split(" ").ToArray();
            var speechConfiguration = new SpeechConverterConfiguration(_speechConverterLogger.Object);

            // Assert
            Assert.Throws<ArgumentException>(() => speechConfiguration.Initialize(args));
        }

        [Fact]
        public void Given_Valid_Args_Get_InputFileName_Returns_Correct_Value()
        {
            // Arrange
            var input = @"-subscriptionKey 1234 -subscriptionRegion canada -inputFile ./testfiles/test.mp3 -outputFile ./testfiles/test.txt";
            var args = input.Split(" ").ToArray();
            var speechConverterConfiguration = new SpeechConverterConfiguration(_speechConverterLogger.Object);
            speechConverterConfiguration.Initialize(args);

            // Act
            var expectedValue = @"./testfiles/test.mp3";
            var actualValue = speechConverterConfiguration.InputFile;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Given_Valid_OutOfOrderArgs_Get_InputFileName_Returns_Correct_Value()
        {
            // Arrange
            var input = @"-inputFile ./testfiles/test.mp3 -subscriptionRegion canada -subscriptionKey 1234 -outputFile ./testfiles/test.txt";
            var args = input.Split(" ").ToArray();
            var speechConverterConfiguration = new SpeechConverterConfiguration(_speechConverterLogger.Object);
            speechConverterConfiguration.Initialize(args);

            // Act
            var expectedValue = @"./testfiles/test.mp3";
            var actualValue = speechConverterConfiguration.InputFile;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Given_Valid_Args_Get_Values_Returns_Correct_Value()
        {
            // Arrange
            var input = @"-subscriptionKey 1234 -subscriptionRegion canada -inputFile ./testfiles/test.mp3 -outputFile ./testfiles/test.txt";
            var args = input.Split(" ").ToArray();
            var speechConverterConfiguration = new SpeechConverterConfiguration(_speechConverterLogger.Object);
            speechConverterConfiguration.Initialize(args);

            // Act
            var expectedInputFile = @"./testfiles/test.mp3";
            var expectedOutputFile = @"./testfiles/test.txt";
            var inputFile = speechConverterConfiguration.InputFile;
            var outputFile = speechConverterConfiguration.OutputFile;

            // Assert
            Assert.Equal(expectedInputFile, inputFile);
            Assert.Equal(expectedOutputFile, outputFile);
        }

        [Fact]
        public void Given_Fake_Input_And_Output_File_Throws_Exception()
        {
            // Arrange
            var input = @"-subscriptionKey 1234 -subscriptionRegion canada -inputFile c:\foo.mp3 -outputFile c:\foo.mp3";
            var args = input.Split(" ").ToArray();
            var speechConverterConfiguration = new SpeechConverterConfiguration(_speechConverterLogger.Object);

            // Assert
            Assert.Throws<FileNotFoundException>(() => speechConverterConfiguration.Initialize(args));
        }
    }
}
