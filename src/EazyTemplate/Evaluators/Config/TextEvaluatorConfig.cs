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
    private readonly Dictionary<string, ITextEvaluatorWrapper> _builtInTypeResolvers = new();

    internal TextEvaluatorConfig() { }

    internal TextEvaluatorConfig(List<ITextEvaluatorWrapper> builtInTypeResolvers)
    {
        foreach (var resolver in builtInTypeResolvers)
            AddBuiltInTypeResolver(resolver.GetResolverType(), resolver);
    }

    /// <summary>
    /// Get's resolver function for requested type.
    /// </summary>
    public ITextEvaluatorWrapper GetForBuiltInType(Type type)
    {
        if (_builtInTypeResolvers.TryGetValue(type.Name, out var result))
            return result;

        return new TextEvaluatorWrapper<object?>(value =>
            value == default 
                ? string.Empty 
                : value.ToString() ?? string.Empty);
    }

    private void AddBuiltInTypeResolver(Type builtInType, ITextEvaluatorWrapper resolverWrapper)
    {
        if (!SupportedTypes.Contains(builtInType))
            throw new EazyTemplateException($"This type is not yet supported.");

        if (!_builtInTypeResolvers.TryAdd(builtInType.Name, resolverWrapper))
            throw new EazyTemplateException($"Could not configure for type {builtInType.Name}.");
    }
}
