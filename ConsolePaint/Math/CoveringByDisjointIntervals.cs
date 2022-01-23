using System.Collections.Immutable;

namespace ConsolePaint.Math;

public class CoveringByDisjointIntervals<TElement>
    where TElement : IComparable<TElement>
{
    private readonly ImmutableArray<((TElement Start, TElement End) Interval, int Index)> _covering;

    public CoveringByDisjointIntervals(IEnumerable<(TElement Start, TElement End)> intervals)
    {
        // TODO: Check
        //      - ordered, continuous etc
        _covering = intervals
            .Select((tuple, index) => ((tuple.Start, tuple.End), index))
            .ToImmutableArray();
    }
    
    // TODO: Binary search for more efficiency
    // TODO: Guard against out of range
    // public int GetCoveringIntervalIndex(TElement element)
    //     => _covering.First(cover => cover.Interval.Start <= element)
}