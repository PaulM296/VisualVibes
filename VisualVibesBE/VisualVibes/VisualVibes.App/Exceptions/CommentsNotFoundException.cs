namespace VisualVibes.App.Exceptions
{
    public class CommentsNotFoundException : Exception
    {
        public CommentsNotFoundException() : base() { }
        public CommentsNotFoundException(string message) : base(message) { }
        public CommentsNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
