namespace EazyTemplate.Evaluators.Config;

/// <inheritdoc />
public class TextEvaluatorConfigBuilder : ITextEvaluatorConfigBuilder
{
    private readonly List<ITextEvaluatorWrapper> _builtInTypeResolvers = new();

    /// <summary>
    /// Gets all already registered types for the builder.
    /// </summary>
    public IReadOnlyList<Type> RegisteredTypes => 
        _builtInTypeResolvers.Select(v => v.GetResolverType()).ToList().AsReadOnly();
    
    /// <inheritdoc />
    public TextEvaluatorConfig Build() => new(_builtInTypeResolvers);

    /// <inheritdoc />
    public void UseTypeResolver<T>(Func<T, string> typeResolver) 
        => _builtInTypeResolvers.Add(new TextEvaluatorWrapper<T>(typeResolver));
}
