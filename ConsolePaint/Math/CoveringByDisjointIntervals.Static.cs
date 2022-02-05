using System.Collections.Immutable;
using Ardalis.GuardClauses;
using ConsolePaint.Utility;

namespace ConsolePaint.Math;

public static class CoveringByDisjointIntervals
{
    public static CoveringByDisjointIntervals<TElement> FromIntervalLengths<TElement>(
        TElement start, Func<TElement, TElement, TElement> addElements, params TElement[] lengths)
        where TElement : IComparable, IComparable<TElement>
        => FromIntervalLengths(start, addElements, lengths.ToImmutableArray());
    
    // TODO: Should we disallow null start?
    public static CoveringByDisjointIntervals<TElement> FromIntervalLengths<TElement>(
        TElement start, Func<TElement, TElement, TElement> addElements, IEnumerable<TElement> lengths)
        where TElement : IComparable, IComparable<TElement>
        => Enumerate.AndApplyGuardingAgainstNull(
            lengths,
            // ReSharper disable once VariableHidesOuterVariable
            lengths =>
            {
                Guard.Against.Null(addElements);
                
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

                return new CoveringByDisjointIntervals<TElement>(intervals);
            });
}