namespace EazyTemplate.Evaluators.Config;

/// <summary>
/// Wrapper around resolver functions that are used by evaluator to get string representation of the passed types.
/// </summary>
public interface ITextEvaluatorWrapper
{
    /// <summary>
    /// Gets the function wrapped.
    /// </summary>
    /// <returns>Function that takes argument of type equal to result of <see cref="GetResolverType"/> and returns
    /// string representation of the type.</returns>
    public Func<dynamic, string> GetResolver();
    
    /// <summary>
    /// Gets the type for which resolver function is registered.
    /// </summary>
    public Type GetResolverType();
}
