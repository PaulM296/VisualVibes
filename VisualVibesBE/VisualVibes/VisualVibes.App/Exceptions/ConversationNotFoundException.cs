namespace VisualVibes.App.Exceptions
{
    public class ConversationNotFoundException : Exception
    {
        public ConversationNotFoundException() : base() { }
        public ConversationNotFoundException(string message) : base(message) { }
        public ConversationNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
