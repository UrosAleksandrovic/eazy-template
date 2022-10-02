namespace EazyTemplate.Evaluators.Config;

internal interface ITextEvaluatorConfigBuilder : ITextEvaluatorConfigurator
{
    /// <summary>
    /// Builds TextEvaluatorConfig that will be used by ITextBuilder when populating text template.
    /// </summary>
    /// <remarks> Once build config cannot be changed.</remarks>
    public TextEvaluatorConfig Build();
}
