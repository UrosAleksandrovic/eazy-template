namespace EazyTemplate.Evaluators.Config;

public class TextEvaluatorConfigBuilder : ITextEvaluatorConfigBuilder
{
    private readonly List<(Type, Func<object?, string>)> _builtInTypeResolvers = new();

    public IReadOnlyList<Type> RegisteredTypes => _builtInTypeResolvers.Select(v => v.Item1).ToList().AsReadOnly();

    public TextEvaluatorConfig Build() => new(_builtInTypeResolvers);

    public void UseTypeResolver<T>(Func<object?, string> typeResolver)
    {
        _builtInTypeResolvers.Add((typeof(T), typeResolver));
    }
}
