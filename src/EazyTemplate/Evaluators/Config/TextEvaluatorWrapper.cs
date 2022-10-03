namespace EazyTemplate.Evaluators.Config;

/// <inheritdoc />
/// <typeparam name="T"> Type for which resolver function is registered</typeparam>
internal class TextEvaluatorWrapper<T> : ITextEvaluatorWrapper
{
    private readonly Func<T, string> _resolver;

    public TextEvaluatorWrapper(Func<T, string> resolver)
    {
        _resolver = resolver;
    }

    /// <inheritdoc />
    public Func<dynamic, string> GetResolver() => input => _resolver.Invoke(input);

    /// <inheritdoc />
    public Type GetResolverType() => typeof(T);
}
