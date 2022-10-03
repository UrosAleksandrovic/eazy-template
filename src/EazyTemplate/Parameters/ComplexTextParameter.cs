using EazyTemplate.Core;
using EazyTemplate.Evaluators;
using EazyTemplate.Evaluators.Config;
using EazyTemplate.Parameters.Config;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace EazyTemplate.Parameters;

public class ComplexTextParameter : TextParameter, ITextEvaluator
{
    private readonly ITextParameterFactory _parameterFactory;

    private string TextTemplate { get; }

    private string DeclaredName { get; }

    public ComplexTextParameter(
        string fullPath,
        string declaredName,
        string textTemplate,
        int locationInTemplate,
        int lengthOfWholeParameter,
        ParametersConfig? paramConfig = default,
        TextEvaluatorConfig? evaluatorConfig = default)
        : base(paramConfig, evaluatorConfig)
    {
        PathFromParent = fullPath;
        LocationInTemplate = locationInTemplate;
        Length = lengthOfWholeParameter;
        TextTemplate = textTemplate;
        DeclaredName = declaredName;

        _parameterFactory = new TextParameterFactory(paramConfig, evaluatorConfig);
    }

    public ComplexTextParameter(
        string textTemplate,
        ParametersConfig? paramConfig = default,
        TextEvaluatorConfig? evaluatorConfig = default)
        : base(paramConfig, evaluatorConfig)
    {
        DeclaredName = "root";
        TextTemplate = textTemplate;
        Length = string.IsNullOrWhiteSpace(textTemplate) ? 0 : textTemplate.Length;

        _parameterFactory = new TextParameterFactory(paramConfig, evaluatorConfig);
    }

    private string ParameterText
        => $"{ParamConfig.OpeningPattern}{PathFromParent}{ParametersConfig.DeclarationPattern}{DeclaredName}"
          + $":{TextTemplate}:{PathFromParent}{ParamConfig.ClosingPattern}";

    public override string Evaluate(object? root, Type rootType)
    {
        if (root == null)
            throw new ArgumentNullException(nameof(root));

        var (complexObject, propType) = FindComplexProperty(root, rootType);
        var propNotFound = complexObject == null && propType == null;
        if (propNotFound && ParamConfig.PopulateUnknownParameters)
            return string.Empty;

        if (propNotFound)
            return ParameterText;

        if (complexObject == default)
            return string.Empty;

        var orderedParameters = GetOrderedChildParameters();
        var parameterValues = new List<string>();

        if (propType!.IsAssignableTo(typeof(IEnumerable)))
        {
            var enumGenericType = propType.IsArray 
                    ? propType.GetElementType()!
                    : propType.GetGenericArguments().Single();
            foreach (var singleObject in ((IEnumerable)complexObject).Cast<object>())
                parameterValues.AddRange(GetNestedValues(singleObject, orderedParameters, enumGenericType));
        }
        else
            parameterValues.AddRange(GetNestedValues(complexObject, orderedParameters, propType));

        return ConstructEndResult(parameterValues);
    }

    public (object?, Type?) FindComplexProperty(object root, Type rootType)
    {
        if (root == null)
            throw new ArgumentNullException(nameof(root));

        if (rootType == null)
            throw new ArgumentNullException(nameof(rootType));

        var propertyPath = PathArray;
        if (propertyPath.Length == 0)
            throw new InvalidPropertyPathException($"Property path must have at least root declared.");

        if (propertyPath.Length == 1 && DeclaredName == "root")
            return (root, rootType);

        object? currentObject = root;
        Type currentType = rootType;
        short currentLevel = 1;
        while (currentLevel < propertyPath.Length && currentObject != null)
        {
            var currentProperty = currentType.GetProperty(propertyPath[currentLevel++]);
            if (currentProperty == null)
                return (null, null);

            if (currentProperty.PropertyType.IsAssignableTo(typeof(IEnumerable)) && currentLevel < propertyPath.Length)
                throw new InvalidPropertyTypeException("ComplexTextResolver does not support enumerable properties on path.");

            currentObject = currentProperty.GetValue(currentObject);
            currentType = currentProperty.PropertyType;
        }

        return (currentObject, currentType);
    }

    [SuppressMessage(
        "Major Code Smell", "S2589:Boolean expressions should not be gratuitous",
        Justification = "False positive")]
    public List<TextParameter> GetOrderedChildParameters()
    {
        var simpleEnumerator = GetSimpleChildrenEnumerator();
        var complexEnumerator = GetComplexChildrenEnumerator();
        bool hasSimpleParameters = simpleEnumerator.MoveNext();
        bool hasComplexParameters = complexEnumerator.MoveNext();

        var parameters = new List<TextParameter>();
        while (hasSimpleParameters || hasComplexParameters)
        {
            if (!hasComplexParameters && hasSimpleParameters)
            {
                parameters.Add(simpleEnumerator.Current);
                hasSimpleParameters = simpleEnumerator.MoveNext();
                continue;
            }

            if (hasComplexParameters && !hasSimpleParameters)
            {
                parameters.Add(complexEnumerator.Current);
                hasComplexParameters = complexEnumerator.MoveNext();
                continue;
            }

            var isSimpleNext = simpleEnumerator.Current.LocationInTemplate < complexEnumerator.Current.LocationInTemplate;
            parameters.Add(isSimpleNext ? simpleEnumerator.Current : complexEnumerator.Current);
            if (isSimpleNext)
                hasSimpleParameters = simpleEnumerator.MoveNext();
            else
                hasComplexParameters = complexEnumerator.MoveNext();
        }

        return parameters;
    }

    private IEnumerator<SimpleTextParameter> GetSimpleChildrenEnumerator()
    {
        if (string.IsNullOrWhiteSpace(TextTemplate))
            throw new EazyTemplateException("No text template is provided.");

        var simpleParameterRegex = new Regex(ParamConfig.SimpleParameterRegex);
        var match = simpleParameterRegex.Match(TextTemplate);

        while (match.Success)
        {
            var simpleParam = _parameterFactory.CreateSimpleParameter(match);

            var fullPathArray = simpleParam.PathArray;
            if (fullPathArray.First() == DeclaredName && fullPathArray.Length > 1)
                yield return simpleParam;

            match = match.NextMatch();
        }
    }

    private IEnumerator<ComplexTextParameter> GetComplexChildrenEnumerator()
    {
        if (string.IsNullOrWhiteSpace(TextTemplate))
            throw new EazyTemplateException("No text template is provided.");

        var complexParameterStartPattern = new Regex(ParamConfig.ComplexParameterOpeningRegex);
        var match = complexParameterStartPattern.Match(TextTemplate);

        while (match.Success)
        {
            var complexParam = _parameterFactory.CreateComplexParameter(match, TextTemplate);

            var fullPathArray = complexParam.PathArray;
            if (fullPathArray.First() == DeclaredName && fullPathArray.Length > 1)
                yield return complexParam;

            match = match.NextMatch();
        }
    }

    private List<string> GetNestedValues(object root, IEnumerable<TextParameter> textParameters, Type rootType)
    {
        var stringResults = new List<string>();
        var lastEndingIndex = 0;
        var textTemplateSpan = TextTemplate.AsSpan();
        foreach (var param in textParameters)
        {
            stringResults.Add(textTemplateSpan[lastEndingIndex..param.LocationInTemplate].ToString());

            stringResults.Add(param.Evaluate(root, rootType));

            lastEndingIndex = param.EndLocationInTemplate;
        }

        stringResults.Add(textTemplateSpan[lastEndingIndex..].ToString());

        return stringResults;
    }

    private string ConstructEndResult(IEnumerable<string> paramValues)
    {
        var stringBuilder = new StringBuilder();
        foreach (var value in paramValues)
            stringBuilder.Append(value);

        return stringBuilder.ToString();
    }
}
