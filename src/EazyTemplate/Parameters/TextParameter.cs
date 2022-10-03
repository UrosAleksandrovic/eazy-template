using EazyTemplate.Parameters.Config;
using EazyTemplate.Evaluators;
using EazyTemplate.Evaluators.Config;

namespace EazyTemplate.Parameters;

public abstract class TextParameter : ITextEvaluator
{
    protected string PathFromParent { get; init; }
    public int LocationInTemplate { get; protected init; }
    protected int Length { get; init; }
    protected ParametersConfig ParamConfig { get; }
    protected TextEvaluatorConfig ResolverConfig { get; }

    public string[] PathArray 
        => string.IsNullOrWhiteSpace(PathFromParent)
        ? Array.Empty<string>() 
        : PathFromParent.Split('.');

    protected string Name => PathArray.Last();
    public int EndLocationInTemplate => LocationInTemplate + Length;

    protected TextParameter(
        ParametersConfig? paramConfig = default,
        TextEvaluatorConfig? evaluatorConfig = default)
    {
        ParamConfig = paramConfig ?? new ParametersConfig();
        ResolverConfig = evaluatorConfig ?? new TextEvaluatorConfig();

        PathFromParent = "root";
        LocationInTemplate = 0;
        Length = 0;
    }

    public abstract string Evaluate(object? root, Type rootType);
}
