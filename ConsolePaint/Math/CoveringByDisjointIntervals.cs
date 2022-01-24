using System.Collections.Immutable;
using Ardalis.GuardClauses;
using ConsolePaint.Math.Extensions;

namespace ConsolePaint.Math;

public class CoveringByDisjointIntervals<TElement> : ICoveringByDisjointIntervals<TElement>
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

        if (intervalsEnumerated.Any(interval => interval.Start.IsGreaterThanOrEqualTo(interval.End)))
            throw new InvalidOperationException(
                "Can't construct covering by disjoint intervals from intervals with start lower than or equal to end");

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

    public static CoveringByDisjointIntervals<TElement> FromIntervalLengths(
        TElement start, Func<TElement, TElement, TElement> addElements, params TElement[] lengths)
        => FromIntervalLengths(start, addElements, lengths.ToImmutableArray());

    // TODO: Check null
    public static CoveringByDisjointIntervals<TElement> FromIntervalLengths(
        TElement start, Func<TElement, TElement, TElement> addElements, IEnumerable<TElement> lengths)
    {
        // TODO: Can we use IEnumerable here instead of List?
        var starts = lengths
            .Aggregate(
                new List<TElement> { start },
                (starts, length) =>
                {
                    starts.Add(addElements(starts.Last(), length));
                    return starts;
                });

        var intervals = starts
            .Zip(starts.Skip(1))
            .Select(tuple => (Start: tuple.First, End: tuple.Second))
            .ToImmutableArray();

        return new(intervals);
    }

    // TODO: Binary search for more efficiency
    public ((TElement Start, TElement End) Interval, int Index) GetCover(TElement element)
    {
        Guard.Against.OutOfRange(element, nameof(element), Start, End);
        return _covering.First(cover => cover.Interval.Start.IsLowerThanOrEqualTo(element)
                                        && cover.Interval.End.IsGreaterThan(element));
    }
}