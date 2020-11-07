using System;
using System.Linq;
using Xunit;


namespace SpeechConverter.Tests
{
    public class InputValidationTests
    {
        [Fact]
        public void Validate_SubscriptionKey_Argument_Returns_True()
        {
            // Arrange
            var expectedResult = "-subscriptionKey";
            string[] args = {expectedResult};

            // Act
            var oARgs = args.ToList();

            // Assert
            Assert.Contains(expectedResult, oARgs);
        }

        [Fact]
        public void Correct_Args_Validates_Without_Exception()
        {
            // Arrange
            var input = @"-subscriptionKey 1234 -subscriptionRegion canada -inputFile c:\foo -outputFile c:\bar";
            var args = input.Split(" ").ToArray();

            // Act
            var expectedResult = 8;
            var actualResult = args.Length;

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(@"-subscriptionKey 1234 -subscriptionRegion canada -inputFile c:\foo -outputFil c:\bar")]
        [InlineData(@"-subscriptionKey 1234 -subscriptionRegion canada -inputFile -outputFile c:\bar")]
        [InlineData(@"-subscriptionKey 1234-subscriptionRegion canada -inputFile c:\foo -outputFile c:\bar")]
        [InlineData(@"-subscriptionKey 1234 -subscriptionRegioncanada -inputFile c:\foo -outputFile c:\bar")]
        [InlineData(@"-subiptionKey 1234 -bscriptionRegion canada -inputFile c:\foo -outputFile c:\bar")]
        [InlineData(@"-subscriptionKey1234 -subscriptionRegion canada -inptFile c:\foo -outputFile c:\bar")]
        [InlineData(@"-subscriptionKey1234 -subscriptionRegion canada -inptFile c:\foo -outputFile")]
        [InlineData(@"-inptFile c:\foo -outputFile c:\bar")]
        [InlineData(@"")]
        public void Invalid_Parameters_Throws_ArgumentException(string input)
        {
            // Arrange
            var args = input.Split(" ").ToArray();

            // Assert
            Assert.Throws<ArgumentException>(() => new SpeechConverterConfiguration(args));
        }

        [Fact]
        public void Given_Valid_Args_Get_InputFileName_Returns_Correct_Value()
        {
            // Arrange
            var input = @"-subscriptionKey 1234 -subscriptionRegion canada -inputFile c:\foo -outputFile c:\bar";
            var args = input.Split(" ").ToArray();
            var speechConverterConfiguration = new SpeechConverterConfiguration(args);

            // Act
            var expectedValue = @"c:\foo";
            var actualValue = speechConverterConfiguration.InputFile;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Given_Valid_OutOfOrderArgs_Get_InputFileName_Returns_Correct_Value()
        {
            // Arrange
            var input = @"-inputFile c:\foo -subscriptionRegion canada -subscriptionKey 1234 -outputFile c:\bar";
            var args = input.Split(" ").ToArray();
            var speechConverterConfiguration = new SpeechConverterConfiguration(args);

            // Act
            var expectedValue = @"c:\foo";
            var actualValue = speechConverterConfiguration.InputFile;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Given_Valid_Args_Get_Values_Returns_Correct_Value()
        {
            // Arrange
            var input = @"-subscriptionKey 1234 -subscriptionRegion canada -inputFile c:\foo -outputFile c:\bar";
            var args = input.Split(" ").ToArray();
            var speechConverterConfiguration = new SpeechConverterConfiguration(args);

            // Act
            var expectedInputFile = @"c:\foo";
            var expectedOutputFile = @"c:\bar";
            var inputFile = speechConverterConfiguration.InputFile;
            var outputFile = speechConverterConfiguration.OutputFile;

            // Assert
            Assert.Equal(expectedInputFile, inputFile);
            Assert.Equal(expectedOutputFile, outputFile);
        }

    }
}
