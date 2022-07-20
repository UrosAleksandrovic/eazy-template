namespace EazyTemplate.Evaluators.Config;

public interface ITextEvaluatorConfigBuilder
{
    /// <summary>
    /// Registers Function for handling type T when populating text template
    /// </summary>
    /// <typeparam name="T">Type for which function is registered.</typeparam>
    /// <param name="typeResolver">
    /// Function that takes object of type T and returns string for text template
    /// </param>
    public void UseTypeResolver<T>(Func<object?, string> typeResolver);

    /// <summary>
    /// Builds TextEvaluatorConfig that will be used by ITextBuilder when populating text template.
    /// </summary>
    /// <remarks> Once build config cannot be changed.</remarks>
    public TextEvaluatorConfig Build();
}
