namespace UserService.Exceptions
{
    public class FailedHttpRequestException : Exception
    {
        public FailedHttpRequestException() { }

        public FailedHttpRequestException(string message)
            : base(message) { }

        public FailedHttpRequestException(string message, Exception inner)
            : base(message, inner) { }
    }
}
