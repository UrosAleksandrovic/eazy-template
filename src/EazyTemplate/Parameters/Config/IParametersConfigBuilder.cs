namespace EazyTemplate.Parameters.Config;

internal interface IParametersConfigBuilder : IParametersConfigurator
{
    /// <summary>
    /// Builds configuration used by text builder.
    /// </summary>
    /// <remarks>
    /// Once built for text builder it cannot be changed.
    /// </remarks>
    ParametersConfig Build();
}
