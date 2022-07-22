using System.Text.RegularExpressions;

namespace EazyTemplate.Parameters.Config;

/// <summary>
/// Configuration used to parse parameters in text template.
/// </summary>
public class ParametersConfig
{
    public string OpeningRegex { get; private set; } = @"\[\[\[";
    public string ClosingRegex { get; private set; } = @"\]\]\]";
    public string OpeningPattern => Regex.Unescape(OpeningRegex);
    public string ClosingPattern => Regex.Unescape(ClosingRegex);
    public bool PopulateUnknownParameters { get; private set; } = false;

    internal const string VariableNamePattern = "[A-Za-z_][A-Za-z0-9_]+";
    internal const string PropertyPattern = $"{VariableNamePattern}(\\.{VariableNamePattern})*";
    internal const string DeclarationPattern = " as ";

    public string SimpleParameterRegex => $@"{OpeningRegex}{PropertyPattern}{ClosingRegex}";
    public string ComplexParameterOpeningRegex
        => $@"{OpeningRegex}{PropertyPattern}{DeclarationPattern}{VariableNamePattern}:";

    public ParametersConfig() { }

    public ParametersConfig(string openingRegex, string closingRegex)
    {
        if (string.IsNullOrWhiteSpace(openingRegex))
            throw new ArgumentNullException(nameof(openingRegex));

        if (string.IsNullOrWhiteSpace(closingRegex))
            throw new ArgumentNullException(nameof(closingRegex));

        OpeningRegex = openingRegex;
        ClosingRegex = closingRegex;
    }

    public void HandleUnknownParameters()
    {
        PopulateUnknownParameters = true;
    }
}
