using System.Collections.Immutable;
using Ardalis.GuardClauses;
using ConsolePaint.Math.Extensions;

namespace ConsolePaint.Math;

// TODO: Documentation - Intervals are right open - [a, b), [b, c), ...
public class CoveringByDisjointIntervals<TElement>
    where TElement : IComparable, IComparable<TElement>
{
    private readonly ImmutableArray<((TElement Start, TElement End) Interval, int Index)> _covering;

    public CoveringByDisjointIntervals(params (TElement Start, TElement End)[] intervals)
        // ReSharper disable once ConstantConditionalAccessQualifier
        : this(intervals?.ToImmutableArray()!)
    {
    }

    public CoveringByDisjointIntervals(IEnumerable<(TElement Start, TElement End)> intervals)
    {
        // TODO: More elegant guarding
        // TODO: Does this cause multi enumeration?
        // ReSharper disable once PossibleMultipleEnumeration
        Guard.Against.Null(intervals, nameof(intervals));

        // ReSharper disable once PossibleMultipleEnumeration
        var intervalsEnumerated = intervals.ToImmutableArray();

        Guard.Against.NullOrEmpty(intervalsEnumerated, nameof(intervals));
        _covering = intervalsEnumerated
            .OrderBy(tuple => tuple.Start)
            .Select((tuple, index) => ((tuple.Start, tuple.End), index))
            .ToImmutableArray();

        var endsWithNextStarts = _covering
            .Zip(_covering.Skip(1))
            .Select(tuple => (tuple.First.Interval.End, NextStart: tuple.Second.Interval.Start));

        if (endsWithNextStarts.Any(tuple => tuple.End.IsEqualTo(tuple.NextStart) is false))
            throw new InvalidOperationException(
                "Can't construct covering by disjoint intervals from discontinuous intervals");
    }

    public TElement Start => _covering.First().Interval.Start;
    public TElement End => _covering.Last().Interval.End;

    // TODO: Binary search for more efficiency
    public ((TElement Start, TElement End) Interval, int Index) GetCover(TElement element)
    {
        Guard.Against.OutOfRange(element, nameof(element), Start, End);
        return _covering.First(cover => cover.Interval.Start.IsLowerThanOrEqualTo(element)
                                        && cover.Interval.End.IsGreaterThan(element));
    }
}