namespace EazyTemplate.Parameters.Config;

/// <summary>
/// Abstraction of the configurator, internally used to instantiate configuration.
/// Parameters Config Configurator is only for configuring how certain situations should be handled by
/// parser and parameters when evaluating.
/// </summary>
public interface IParametersConfigurator
{
    /// <summary>
    /// Sets opening and closing regex that are used when parameters are parsed from text template.
    /// </summary>
    void UseOpeningAndClosingRegex(string openingRegex, string closingRegex);
    
    /// <summary>
    /// If parameter cannot be found in the root project then it is populated with empty string.
    /// </summary>
    void UseEmptyStringForUnKnownProperties();
}
