using System.Text.RegularExpressions;

namespace EazyTemplate.Parameters;

/// <summary>
/// Factory used to produce parameters from regex matches in text templates.
/// </summary>
public interface ITextParameterFactory
{
    ComplexTextParameter CreateComplexParameter(Match regexMatch, string parentTemplate);
    SimpleTextParameter CreateSimpleParameter(Match regexMatch);
}
