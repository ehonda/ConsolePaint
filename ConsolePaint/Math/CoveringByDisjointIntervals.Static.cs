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
        var boundaryPoints = lengths
            .Aggregate(
                (BoundaryPoints: Enumerable.Repeat(start, 1), LastBoundaryPoint: start),
                (accumulator, length) =>
                {
                    var nextBoundaryPoint = addElements(accumulator.LastBoundaryPoint, length);
                    return (accumulator.BoundaryPoints.Append(nextBoundaryPoint), nextBoundaryPoint);
                })
            .BoundaryPoints
            .ToImmutableArray();

        var intervals = boundaryPoints
            .Zip(boundaryPoints.Skip(1))
            .Select(tuple => (Start: tuple.First, End: tuple.Second))
            .ToImmutableArray();

        return new(intervals);
    }
}