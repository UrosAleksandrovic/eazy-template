using System.Text.RegularExpressions;

namespace EazyTemplate.Parameters;

/// <summary>
/// Factory used to produce parameters from regex matches in text templates.
/// </summary>
public interface ITextParameterFactory
{
    /// <summary>
    /// Instantiate ComplexTextParameter based on it's regex match in the template and template from it's parent
    /// </summary>
    /// <param name="regexMatch">Regex Match found in the template of the parent</param>
    /// <param name="parentTemplate">Template from parent complex parameter</param>
    ComplexTextParameter CreateComplexParameter(Match regexMatch, string parentTemplate);

    /// <summary>
    /// Instantiate SimpleTextParameter based on it's regex match.
    /// </summary>
    /// <param name="regexMatch">Regex Match found in the template of the parent parameter</param>
    /// <returns></returns>
    SimpleTextParameter CreateSimpleParameter(Match regexMatch);
}
