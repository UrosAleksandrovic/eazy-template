namespace EazyTemplate.Evaluators.Config;

public interface ITextEvaluatorWrapper
{
    public Func<dynamic, string> GetResolver();
    public Type GetResolverType();
}
