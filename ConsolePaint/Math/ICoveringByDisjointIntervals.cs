namespace ConsolePaint.Math;

// TODO: Documentation - Intervals are right open - [a, b), [b, c), ...
public interface ICoveringByDisjointIntervals<TElement>
    where TElement : IComparable, IComparable<TElement>
{
    public TElement Start { get; }
    public TElement End { get; }

    public ((TElement Start, TElement End) Interval, int Index) GetCover(TElement element);
}