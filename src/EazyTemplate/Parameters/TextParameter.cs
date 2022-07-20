using EazyTemplate.Parameters.Config;
using EazyTemplate.Evaluators;
using EazyTemplate.Evaluators.Config;

namespace EazyTemplate.Parameters;

public abstract class TextParameter : ITextEvaluator
{
    public string PathFromParent { get; protected set; }
    public int LocationInTemplate { get; protected set; }
    public int Length { get; protected set; }
    public ParametersConfig ParamConfig { get; protected set; }
    public TextEvaluatorConfig ResolverConfig { get; protected set; }

    public string[] PathArray 
        => string.IsNullOrWhiteSpace(PathFromParent)
        ? Array.Empty<string>() 
        : PathFromParent.Split('.');

    public string Name => PathArray.Last();
    public int EndLocationInTemplate => LocationInTemplate + Length;

    protected TextParameter(
        ParametersConfig? paramConfig = default,
        TextEvaluatorConfig? evaluatorConfig = default)
    {
        ParamConfig = paramConfig == default ? new ParametersConfig() : paramConfig;
        ResolverConfig = evaluatorConfig == default ? new TextEvaluatorConfig() : evaluatorConfig;

        PathFromParent = "root";
        LocationInTemplate = 0;
        Length = 0;
    }

    public abstract string Evaluate(object? root, Type rootType);
}
