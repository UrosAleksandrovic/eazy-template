namespace EazyTemplate.Evaluators.Config;

public class TextEvaluatorConfigBuilder : ITextEvaluatorConfigBuilder
{
    private readonly List<(Type Type, Func<object?, string> ResolverFunction)> _builtInTypeResolvers = new();

    public IReadOnlyList<Type> RegisteredTypes => _builtInTypeResolvers.Select(v => v.Type).ToList().AsReadOnly();

    public TextEvaluatorConfig Build() => new(_builtInTypeResolvers);

    public void UseTypeResolver<T>(Func<object?, string> typeResolver)
    {
        _builtInTypeResolvers.Add((typeof(T), typeResolver));
    }
}
