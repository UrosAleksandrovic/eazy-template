namespace EazyTemplate.Evaluators.Config;

public class TextEvaluatorConfigBuilder : ITextEvaluatorConfigBuilder
{
    private readonly List<ITextEvaluatorWrapper> _builtInTypeResolvers = new();

    public IReadOnlyList<Type> RegisteredTypes => 
        _builtInTypeResolvers.Select(v => v.GetResolverType()).ToList().AsReadOnly();

    public TextEvaluatorConfig Build() => new(_builtInTypeResolvers);

    public void UseTypeResolver<T>(Func<T, string> typeResolver) 
        => _builtInTypeResolvers.Add(new TextEvaluatorWrapper<T>(typeResolver));
}
