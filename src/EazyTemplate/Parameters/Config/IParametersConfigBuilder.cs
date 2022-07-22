namespace EazyTemplate.Parameters.Config;

public interface IParametersConfigBuilder
{
    /// <summary>
    /// Sets opening and closing regex that are used when parameters are parsed from text template.
    /// </summary>
    void UseOpeninAndClosingRegex(string openingRegex, string closingRegex);

    /// <summary>
    /// If parameter cannot be found in the root project then it is populated with empty string.
    /// </summary>
    void UseEmptyStringForUnKnownProperties();

    /// <summary>
    /// Builds configuration used by text builder.
    /// </summary>
    /// <remarks>
    /// Once built for text builder it cannot be changed.
    /// </remarks>
    ParametersConfig Build();
}
