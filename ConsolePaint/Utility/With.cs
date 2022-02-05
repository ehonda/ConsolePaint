using System.Collections.Immutable;
using Ardalis.GuardClauses;

// TODO: Better namespace
namespace ConsolePaint.Utility;

// TODO: Better names!
// TODO: As extension on IEnumerable<T>?
public static class With
{
    public static TOutput NotNullAndEnumerated<TOutput, TElement>(
        IEnumerable<TElement> elements,
        Func<ImmutableArray<TElement>, TOutput> transformation)
    {
        // ReSharper disable once ConstantConditionalAccessQualifier
        var elementsEnumerated = elements?.ToImmutableArray();

        Guard.Against.Null(elementsEnumerated);

        return transformation(elementsEnumerated.Value);
    }

    public static TOutput NotNullOrEmptyAndEnumerated<TOutput, TElement>(
        IEnumerable<TElement> elements,
        Func<ImmutableArray<TElement>, TOutput> transformation)
    {
        // ReSharper disable once ConstantConditionalAccessQualifier
        var elementsEnumerated = elements?.ToImmutableArray();

        Guard.Against.NullOrEmpty(elementsEnumerated as IEnumerable<TElement>, nameof(elements));

        return transformation(elementsEnumerated.Value);
    }
}