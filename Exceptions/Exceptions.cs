namespace PrinterAccounter.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message) { }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    public class FileValidationException : Exception
    {
        public FileValidationException(string message) : base(message) { }
    }
}
