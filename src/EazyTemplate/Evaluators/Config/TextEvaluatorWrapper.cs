namespace EazyTemplate.Evaluators.Config;

internal class TextEvaluatorWrapper<T> : ITextEvaluatorWrapper
{
    private readonly Func<T, string> _resolver;

    public TextEvaluatorWrapper(Func<T, string> resolver)
    {
        _resolver = resolver;
    }

    public Func<dynamic, string> GetResolver() => input => _resolver.Invoke(input);

    public Type GetResolverType() => typeof(T);
}
