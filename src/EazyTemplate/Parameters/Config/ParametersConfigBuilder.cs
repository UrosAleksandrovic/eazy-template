namespace EazyTemplate.Parameters.Config;

public class ParametersConfigBuilder : IParametersConfigBuilder
{
    private string? _openingRegex;
    private string? _closingRegex;

    public void UseOpeninAndClosingRegex(string openingRegex, string closingRegex)
    {
        _openingRegex = openingRegex;
        _closingRegex = closingRegex;
    }

    public ParametersConfig Build()
    {
        if (_openingRegex != null && _closingRegex != null)
            return new ParametersConfig(_openingRegex, _closingRegex);

        return new ParametersConfig();
    }
}
