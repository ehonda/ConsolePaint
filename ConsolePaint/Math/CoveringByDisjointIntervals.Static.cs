using System.Collections.Immutable;

namespace ConsolePaint.Math;

public static class CoveringByDisjointIntervals
{
    public static CoveringByDisjointIntervals<TElement> FromIntervalLengths<TElement>(
        TElement start, Func<TElement, TElement, TElement> addElements, params TElement[] lengths)
        where TElement : IComparable, IComparable<TElement>
        => FromIntervalLengths(start, addElements, lengths.ToImmutableArray());

    // TODO: Check null
    public static CoveringByDisjointIntervals<TElement> FromIntervalLengths<TElement>(
        TElement start, Func<TElement, TElement, TElement> addElements, IEnumerable<TElement> lengths)
        where TElement : IComparable, IComparable<TElement>
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
}