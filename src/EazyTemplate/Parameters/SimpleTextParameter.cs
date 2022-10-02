using EazyTemplate.Core;
using EazyTemplate.Evaluators.Config;
using EazyTemplate.Parameters.Config;
using System.Collections;
using System.Reflection;
using static EazyTemplate.Core.Constants;

namespace EazyTemplate.Parameters;

public class SimpleTextParameter : TextParameter
{
    internal SimpleTextParameter(
        string pathFromParent,
        int locationInTemplate,
        int length,
        ParametersConfig? config = default,
        TextEvaluatorConfig? evaluatorConfig = default)
        : base(config, evaluatorConfig)
    {
        PathFromParent = pathFromParent;
        LocationInTemplate = locationInTemplate;
        Length = length;
    }

    private string ParameterText => $"{ParamConfig.OpeningPattern}{PathFromParent}{ParamConfig.ClosingPattern}";

    public (object?, Type?) FindSimpleProperty(object root, Type rootType)
    {
        if (root == null)
            throw new ArgumentNullException(nameof(root));

        if (rootType == null)
            throw new ArgumentNullException(nameof(rootType));

        var propertyPath = PathArray;
        if (propertyPath.Length <= 1)
            throw new InvalidPropertyPathException($"Property path must have root attribute.");

        object? currentObject = root;
        Type currentType = rootType;
        short currentLevel = 1;
        while (currentLevel < propertyPath.Length && currentObject != null)
        {
            var currentProperty = currentType.GetProperty(propertyPath[currentLevel++]);
            if (currentProperty == null)
                return (null, null);

            if (currentProperty.PropertyType.IsAssignableTo(typeof(IEnumerable)) && currentLevel < propertyPath.Length)
                throw new InvalidPropertyTypeException("SimpleTextResolver does not support enumerable properties on path.");
            
            currentObject = currentProperty.GetValue(currentObject);
            currentType = currentProperty.PropertyType;
        }

        if (currentObject != null && !SupportedTypes.Contains(currentType))
            throw new InvalidPropertyTypeException($"Property type {currentType} is not supported for by SimpleTextResolver.");

        return (currentObject, currentType);
    }

    public override string Evaluate(object? root, Type rootType)
    {
        if (root == default)
            return ParameterText;

        return GetPropertyValue(root, rootType);
    }

    private string GetPropertyValue(object root, Type rootType)
    {
        if (SupportedTypes.Contains(rootType) && Name.ToLowerInvariant() == "value")
            return ResolverConfig.GetForBuiltInType(rootType).GetResolver().Invoke(root);

        var (evaluatedObject, evaluatedType) = FindSimpleProperty(root, rootType);
        var propNotFound = evaluatedObject == null && evaluatedType == null;

        if (propNotFound && ParamConfig.PopulateUnknownParameters)
            return string.Empty;

        if (propNotFound)
            return ParameterText;

        return ResolverConfig.GetForBuiltInType(evaluatedType!).GetResolver().Invoke(evaluatedObject!);
    }
}