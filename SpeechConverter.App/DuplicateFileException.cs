using System;

namespace SpeechConverter.App
{
    public sealed class DuplicateFileException : Exception
    {
        public DuplicateFileException()
        {
            
        }

        public DuplicateFileException(string? message, string? file1, string? file2)
            : base(message)
        {
            
        }

        public DuplicateFileException(string? message, string? file1, string? file2, Exception? innerException)
            : base(message, innerException)
        {
            
        }
    }
}