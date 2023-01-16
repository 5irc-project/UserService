namespace UserService.Exceptions
{
    public class UserDBUpdateException : Exception
    {
        public UserDBUpdateException() { }

        public UserDBUpdateException(string message)
            : base(message) { }

        public UserDBUpdateException(string message, Exception inner)
            : base(message, inner) { }
    }
}
