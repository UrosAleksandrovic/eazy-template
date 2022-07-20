namespace EazyTemplate.Evaluators;

internal interface ITextEvaluator
{
    string Evaluate(object? root, Type rootType);
}
