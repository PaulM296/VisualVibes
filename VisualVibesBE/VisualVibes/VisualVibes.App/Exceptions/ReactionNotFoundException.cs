namespace VisualVibes.App.Exceptions
{
    public class ReactionNotFoundException : Exception
    {
        public ReactionNotFoundException() : base() { }
        public ReactionNotFoundException(string message) : base(message) { }
        public ReactionNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
