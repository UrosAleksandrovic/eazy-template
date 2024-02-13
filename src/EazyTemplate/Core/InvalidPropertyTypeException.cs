namespace EazyTemplate.Core;

[Serializable]
public class InvalidPropertyTypeException : EazyTemplateException
{
    public InvalidPropertyTypeException() { }

    public InvalidPropertyTypeException(string message) : base(message) { }

    public InvalidPropertyTypeException(string? message, Exception? innerException) : base(message, innerException) { }
}
