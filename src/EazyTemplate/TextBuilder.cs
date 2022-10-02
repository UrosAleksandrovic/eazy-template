using EazyTemplate.Evaluators.Config;
using EazyTemplate.Parameters;
using EazyTemplate.Parameters.Config;

namespace EazyTemplate;

public class TextBuilder : ITextBuilder
{
    private string _textTemplate = string.Empty;
    private readonly ParametersConfigBuilder _paramConfigBuilder = new();
    private readonly TextEvaluatorConfigBuilder _textResolverConfigBuilder = new();

    public void HasTemplate(string textTemplate)
    {
        if (string.IsNullOrWhiteSpace(textTemplate))
            throw new ArgumentNullException(nameof(textTemplate));

        _textTemplate = textTemplate;
    }

    public string BuildBody<T>(T root) where T : class
    {
        var _rootElement = new ComplexTextParameter(
            _textTemplate,
            _paramConfigBuilder.Build(),
            _textResolverConfigBuilder.Build());

        return _rootElement.Evaluate(root, typeof(T));
    }

    public string BuildBody(object root)
    {
        var _rootElement = new ComplexTextParameter(
            _textTemplate,
            _paramConfigBuilder.Build(),
            _textResolverConfigBuilder.Build());

        return _rootElement.Evaluate(root, root.GetType());
    }

    public void UseParametersConfiguration(Action<IParametersConfigBuilder> configBuilder)
    {
        configBuilder.Invoke(_paramConfigBuilder);
    }

    public void UseTextEvaluatorConfig(Action<ITextEvaluatorConfigurator> configurator)
    {
        configurator.Invoke(_textResolverConfigBuilder);
    }
}
