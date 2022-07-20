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

    public (object?, PropertyInfo) FindSimpleProperty(object root, Type rootType)
    {
        if (root == null)
            throw new ArgumentNullException(nameof(root));

        if (rootType == null)
            throw new ArgumentNullException(nameof(rootType));

        var propertyPath = PathArray;
        if (propertyPath.Length <= 1)
            throw new InvalidPropertyPathException($"Property path must have root attribute.");

        object? currentObject = root;
        PropertyInfo? currentProperty = null;
        short currentLevel = 1;
        while (currentLevel < propertyPath.Length && currentObject != null)
        {
            currentProperty = rootType.GetProperty(propertyPath[currentLevel++]);
            if (currentProperty == null)
                throw new InvalidPropertyPathException($"Property {PathFromParent} could not be found.");

            if (currentProperty.PropertyType.IsAssignableTo(typeof(IEnumerable)) && currentLevel < propertyPath.Length)
                throw new InvalidPropertyTypeException("SimpleTextResolver does not support enumerable properties.");

            currentObject = currentProperty.GetValue(currentObject);
        }

        if (currentObject != null && !SupportedTypes.Contains(currentProperty!.PropertyType))
            throw new InvalidPropertyTypeException($"Property type {currentProperty.PropertyType} is not supported for by SimpleTextResolver.");

        return (currentObject, currentProperty!);
    }

    public override string Evaluate(object? root, Type rootType)
    {
        if (root == default)
            return string.Empty;

        return GetPropertyValue(root, rootType);
    }

    private string GetPropertyValue(object root, Type rootType)
    {
        if (SupportedTypes.Contains(rootType) && Name.ToLowerInvariant() == "value")
            return ResolverConfig.GetForBuiltInType(rootType).Invoke(root);

        var (evaluatedObject, evaluatedProperty) = FindSimpleProperty(root, rootType);
        return ResolverConfig.GetForBuiltInType(evaluatedProperty.PropertyType).Invoke(evaluatedObject);
    }
}