using System.Runtime.Serialization;

namespace EazyTemplate.Core;

[Serializable]
public class InvalidPropertyPathException : EazyTemplateException
{
    public InvalidPropertyPathException() { }

    public InvalidPropertyPathException(string message) : base(message) { }

    public InvalidPropertyPathException(string? message, Exception? innerException) : base(message, innerException) { }

    protected InvalidPropertyPathException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
