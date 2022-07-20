using EazyTemplate.Evaluators.Config;
using EazyTemplate.Parameters;
using EazyTemplate.Parameters.Config;

namespace EazyTemplate;

public class TextBuilder : ITextBuilder
{
    private string? _textTemplate;
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
        if (_textTemplate == default)
            throw new InvalidOperationException("Template has to be provided to the builder.");

        var _rootElement = new ComplexTextParameter(_textTemplate, _paramConfigBuilder.Build(), _textResolverConfigBuilder.Build());

        return _rootElement.Evaluate(root, typeof(T));
    }

    public void UseParametersConfiguration(Action<IParametersConfigBuilder> configBuilder)
    {
        configBuilder.Invoke(_paramConfigBuilder);
    }

    public void UseTextEvaluatorConfig(Action<ITextEvaluatorConfigBuilder> configBuilder)
    {
        configBuilder.Invoke(_textResolverConfigBuilder);
    }
}
