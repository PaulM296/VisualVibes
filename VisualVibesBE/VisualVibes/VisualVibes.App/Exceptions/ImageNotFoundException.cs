﻿namespace VisualVibes.App.Exceptions
{
    public class ImageNotFoundException : Exception
    {
        public ImageNotFoundException() : base() { }
        public ImageNotFoundException(string message) : base(message) { }
        public ImageNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
