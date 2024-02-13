namespace EazyTemplate.Core;

[Serializable]
public class EazyTemplateException : Exception
{
    public EazyTemplateException() { }

    public EazyTemplateException(string message) : base(message) { }

    public EazyTemplateException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
