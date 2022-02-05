using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;

// TODO: Better namespace
namespace ConsolePaint.Utility;

// TODO: Could this be realized as some sort of fluent / builder syntax?
public static class Enumerate
{
    public static TOutput AndApply<TOutput, TElement>(
        IEnumerable<TElement> elements,
        Func<ImmutableArray<TElement>, TOutput> function)
        => function(elements.ToImmutableArray());
    
    public static TOutput AndApplyGuardingAgainstNull<TOutput, TElement>(
        IEnumerable<TElement> elements,
        Func<ImmutableArray<TElement>, TOutput> function,
        [CallerArgumentExpression("elements")] string? parameterName = null)
    {
        // ReSharper disable once ConstantConditionalAccessQualifier
        var elementsEnumerated = elements?.ToImmutableArray();

        Guard.Against.Null(elementsEnumerated, parameterName);

        return function(elementsEnumerated.Value);
    }

    public static TOutput AndApplyGuardingAgainstNullOrEmpty<TOutput, TElement>(
        IEnumerable<TElement> elements,
        Func<ImmutableArray<TElement>, TOutput> function,
        [CallerArgumentExpression("elements")] string? parameterName = null)
    {
        // ReSharper disable once ConstantConditionalAccessQualifier
        var elementsEnumerated = elements?.ToImmutableArray();

        Guard.Against.NullOrEmpty(
            elementsEnumerated as IEnumerable<TElement>,
            parameterName ?? nameof(elements));

        return function(elementsEnumerated.Value);
    }
}