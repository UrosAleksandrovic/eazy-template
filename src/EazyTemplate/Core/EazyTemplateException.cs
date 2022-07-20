using System.Runtime.Serialization;

namespace EazyTemplate.Core;

[Serializable]
public class EazyTemplateException : Exception
{
    public EazyTemplateException() { }

    public EazyTemplateException(string? message) : base(message) { }

    public EazyTemplateException(string? message, Exception? innerException)
        : base(message, innerException) { }

    protected EazyTemplateException(SerializationInfo info, StreamingContext context) 
        : base(info, context) { }
}
