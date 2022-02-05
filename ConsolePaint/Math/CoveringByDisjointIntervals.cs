using System.Collections.Immutable;
using Ardalis.GuardClauses;
using ConsolePaint.Math.Extensions;
using ConsolePaint.Utility;

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
        _covering = Enumerate.AndApplyGuardingAgainstNullOrEmpty(
            intervals,
            // ReSharper disable once VariableHidesOuterVariable
            intervals =>
            {
                if (intervals.Any(interval => interval.Start.IsGreaterThanOrEqualTo(interval.End)))
                    throw new InvalidOperationException(
                        "Can't construct covering by disjoint intervals from intervals with start lower than or equal to end");

                var covering = intervals
                    .OrderBy(tuple => tuple.Start)
                    .Select((tuple, index) => (Interval: (tuple.Start, tuple.End), Index: index))
                    .ToImmutableArray();

                var endsWithNextStarts = covering
                    .Zip(covering.Skip(1))
                    .Select(tuple => (tuple.First.Interval.End, NextStart: tuple.Second.Interval.Start));

                if (endsWithNextStarts.Any(tuple => tuple.End.IsEqualTo(tuple.NextStart) is false))
                    throw new InvalidOperationException(
                        "Can't construct covering by disjoint intervals from discontinuous intervals");

                return covering;
            });
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