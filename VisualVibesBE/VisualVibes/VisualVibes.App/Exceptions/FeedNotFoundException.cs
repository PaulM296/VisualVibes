namespace VisualVibes.App.Exceptions
{
    public class FeedNotFoundException : Exception
    {
        public FeedNotFoundException() : base() { }
        public FeedNotFoundException(string message) : base(message) { }
        public FeedNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
