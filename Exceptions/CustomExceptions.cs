namespace PrinterAccounter.Exceptions
{
    internal class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message) { }
    }

    internal class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    internal class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    internal class FileValidationException : Exception
    {
        public FileValidationException(string message) : base(message) { }
    }
}
