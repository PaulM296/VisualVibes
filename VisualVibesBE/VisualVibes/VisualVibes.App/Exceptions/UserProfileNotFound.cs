namespace VisualVibes.App.Exceptions
{
    public class UserProfileNotFound : Exception
    {
        public UserProfileNotFound() : base() { }
        public UserProfileNotFound(string message) : base(message) { }
        public UserProfileNotFound(string message, Exception innerException) : base(message, innerException) { }
    }
}
