namespace EazyTemplate.Parameters.Config;

/// <inheritdoc />
public class ParametersConfigBuilder : IParametersConfigBuilder
{
    private string? _openingRegex;
    private string? _closingRegex;
    private bool _emptyStringForUnknownProperties = false;

    public void UseOpeningAndClosingRegex(string openingRegex, string closingRegex)
    {
        _openingRegex = openingRegex;
        _closingRegex = closingRegex;
    }

    /// <inherithdoc />
    public void UseEmptyStringForUnKnownProperties()
    {
        _emptyStringForUnknownProperties = true;
    }

    /// <inherithdoc />
    public ParametersConfig Build()
    {
        var resultConfig = new ParametersConfig();

        if (_openingRegex != null && _closingRegex != null)
            resultConfig= new ParametersConfig(_openingRegex, _closingRegex);

        if (_emptyStringForUnknownProperties)
            resultConfig.HandleUnknownParameters();

        return resultConfig;
    }
}
