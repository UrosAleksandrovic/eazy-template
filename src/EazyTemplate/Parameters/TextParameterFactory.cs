using EazyTemplate.Evaluators.Config;
using EazyTemplate.Parameters.Config;
using System.Text.RegularExpressions;

namespace EazyTemplate.Parameters;

internal class TextParameterFactory : ITextParameterFactory
{
    private readonly ParametersConfig _paramConfig;
    private readonly TextEvaluatorConfig _evaluatorConfig;

    public TextParameterFactory(ParametersConfig? paramConfig = null, TextEvaluatorConfig? evaluatorsConfig = null)
    {
        _paramConfig = paramConfig ?? new ParametersConfig();
        _evaluatorConfig = evaluatorsConfig ?? new TextEvaluatorConfig();
    }

    public ComplexTextParameter CreateComplexParameter(Match regexMatch, string parentTemplate)
    {
        var (fullPath, declaredName) = ExtractPathAndName(regexMatch.ValueSpan);
        var endingPattern = ConstructEndingPatternForPath(fullPath);

        var complexParamBodyEndIndex = parentTemplate.IndexOf(endingPattern, regexMatch.Index);
        var complexParamBodyStartIndex = regexMatch.Index + regexMatch.Length;
        var complexParamBody = parentTemplate[complexParamBodyStartIndex..complexParamBodyEndIndex];

        return new ComplexTextParameter(
            fullPath,
            declaredName,
            complexParamBody,
            regexMatch.Index,
            complexParamBodyEndIndex + endingPattern.Length - regexMatch.Index,
            _paramConfig,
            _evaluatorConfig);
    }

    public SimpleTextParameter CreateSimpleParameter(
        Match regexMatch)
        => new(
            regexMatch.ValueSpan[_paramConfig.OpeningPattern.Length..^_paramConfig.ClosingPattern.Length].ToString(),
            regexMatch.Index,
            regexMatch.Length,
            _paramConfig,
            _evaluatorConfig);

    private (string FullPath, string DeclaredName) ExtractPathAndName(ReadOnlySpan<char> parameterPattern)
    {
        var indexOfDeclaration = parameterPattern.IndexOf(ParametersConfig.DeclarationPattern);

        return (
            parameterPattern[_paramConfig.OpeningPattern.Length..indexOfDeclaration].ToString(),
            parameterPattern[(indexOfDeclaration + ParametersConfig.DeclarationPattern.Length)..^1].ToString());
    }

    private string ConstructEndingPatternForPath(string fullPath) 
        => $":{fullPath}{_paramConfig.ClosingPattern}";

}
