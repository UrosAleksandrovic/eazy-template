namespace EazyTemplate.Evaluators.Config;

/// <summary>
/// Abstraction of the configurator, internally used to instantiate configuration.
/// Text Evaluator Config Configurator is only for directives on how to resolve certain types and properties to end result.
/// </summary>
public interface ITextEvaluatorConfigurator
{
    /// <summary>
    /// Registers Function for handling type T when populating text template
    /// </summary>
    /// <typeparam name="T">Type for which function is registered.</typeparam>
    /// <param name="typeResolver">
    /// Function that takes object of type T and returns string for text template.
    /// If two same types are added build will fail and throw exception.
    /// </param>
    public void UseTypeResolver<T>(Func<T, string> typeResolver);
}
