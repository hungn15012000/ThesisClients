namespace MiddleWare.Exceptions
{
    public class NotSupportedNamespaceException : Exception
    {
        public NotSupportedNamespaceException(string message) : base(message) { }
    }
}