namespace ConsolePaint.Math;

// TODO: Better name
public class PeriodicCoveringByDisjointIntervals<TElement> 
    : ICoveringByDisjointIntervals<TElement>
    where TElement : IComparable, IComparable<TElement>
{
    private readonly CoveringByDisjointIntervals<TElement> _covering;
    // TODO: Drop Elements suffix?
    private readonly Func<TElement, TElement, TElement> _subtractElements;
    private readonly Func<TElement, TElement, double> _divideElements;
    private readonly Func<double, TElement, TElement> _multiplyElements;

    public PeriodicCoveringByDisjointIntervals(
        CoveringByDisjointIntervals<TElement> covering,
        Func<TElement, TElement, TElement> subtractElements,
        Func<TElement, TElement, double> divideElements,
        Func<double, TElement, TElement> multiplyElements)
    {
        _covering = covering;
        _subtractElements = subtractElements;
        _divideElements = divideElements;
        _multiplyElements = multiplyElements;
    }


    public TElement Start => _covering.Start;
    public TElement End => _covering.End;

    public ((TElement Start, TElement End) Interval, int Index) GetCover(TElement element)
    {
        // TODO: Better variable names
        var periodLength = _subtractElements(End, Start);
        var periods = (int) _divideElements(element, periodLength);
        var translatedElement = _subtractElements(
            element,
            _multiplyElements(periods, periodLength));

        return _covering.GetCover(translatedElement);
    }
}