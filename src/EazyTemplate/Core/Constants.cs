namespace EazyTemplate.Core;

internal static class Constants
{
    public static readonly Type[] SupportedTypes =
    {
        typeof(short),
        typeof(int),
        typeof(long),
        typeof(ushort),
        typeof(uint),
        typeof(ulong),
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(char),
        typeof(string),
        typeof(bool),
        typeof(byte),
        typeof(sbyte),
        typeof(string),
        typeof(DateTime)
    };

    public static readonly Type[] SupportedEnumerableTypes =
    {
        typeof(string)
    };
}
