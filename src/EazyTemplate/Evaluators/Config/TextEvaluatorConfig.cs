using EazyTemplate.Core;
using static EazyTemplate.Core.Constants;

namespace EazyTemplate.Evaluators.Config;

/// <summary>
/// Configuration that provides resolver functions to transfer from Built in types to string values for template.
/// </summary>
/// <remarks>
/// Default resolver function calls ToString and if default returns empty string.
/// </remarks>
public class TextEvaluatorConfig
{
    private readonly Dictionary<string, Func<object?, string>> _builtInTypeResolvers = new();

    internal TextEvaluatorConfig() { }

    internal TextEvaluatorConfig(List<(Type, Func<object?, string>)> builtInTypeResolvers)
    {
        foreach (var resolver in builtInTypeResolvers)
            AddBuiltInTypeResolver(resolver.Item1, resolver.Item2);
    }

    /// <summary>
    /// Get's resolver function for requested type.
    /// </summary>
    public Func<object?, string> GetForBuiltInType(Type type)
    {
        if (_builtInTypeResolvers.TryGetValue(type.Name, out var result))
            return result;

        return value => value == default ? string.Empty : value.ToString() ?? string.Empty;
    }

    private void AddBuiltInTypeResolver(Type builtInType, Func<object?, string> resolveFunction)
    {
        if (!SupportedTypes.Contains(builtInType))
            throw new EazyTemplateException($"This type is not yet supported.");

        if (!_builtInTypeResolvers.TryAdd(builtInType.Name, resolveFunction))
            throw new EazyTemplateException($"Could not configure for type {builtInType.Name}.");
    }
}
