namespace UserService.Exceptions
{
    public class UserDBCreationException : Exception
    {
        public UserDBCreationException() { }

        public UserDBCreationException(string message)
            : base(message) { }

        public UserDBCreationException(string message, Exception inner)
            : base(message, inner) { }
    }
}
