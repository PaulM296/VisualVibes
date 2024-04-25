namespace VisualVibes.App.Exceptions
{
    public class MessageNotFoundException : Exception
    {
        public MessageNotFoundException() : base() { }
        public MessageNotFoundException(string message) : base(message) { }
        public MessageNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
