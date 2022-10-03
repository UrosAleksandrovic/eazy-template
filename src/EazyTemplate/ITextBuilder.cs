using EazyTemplate.Evaluators.Config;
using EazyTemplate.Parameters.Config;

namespace EazyTemplate;

public interface ITextBuilder
{
    /// <summary>
    /// Populates text template with properties from root object.
    /// </summary>
    string BuildBody<T>(T root) where T : class;

    /// <summary>
    /// Populates text template with properties from root anonymous object.
    /// </summary>
    string BuildBody(object root);

    /// <summary>
    /// Sets text template to use when building result
    /// </summary>
    void HasTemplate(string textTemplate);

    /// <summary>
    /// Configures parameters config.
    /// </summary>
    void UseParametersConfiguration(Action<IParametersConfigurator> configBuilder);

    /// <summary>
    /// Configures evaluators config.
    /// </summary>
    void UseTextEvaluatorConfig(Action<ITextEvaluatorConfigurator> configurator);
}
