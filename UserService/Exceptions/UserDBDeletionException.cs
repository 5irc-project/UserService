namespace UserService.Exceptions
{
    public class UserDBDeletionException : Exception
    {
        public UserDBDeletionException() { }

        public UserDBDeletionException(string message)
            : base(message) { }

        public UserDBDeletionException(string message, Exception inner)
            : base(message, inner) { }
    }
}
