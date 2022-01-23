using System.Collections.Immutable;

namespace ConsolePaint.Controls;

// TODO: States immutable, oscillator tracks elapsed? We don't want to be able to initialize
// TODO: with states that have elapsed time, nor do we want the outside to be able to advance time in state 
public class TimedOscillator<TTimedState> where TTimedState : TimedState
{
    // TODO: Use another mapping type?
    private readonly IImmutableDictionary<(TimeSpan Start, TimeSpan End), int> _indexMap;

    private readonly TimeSpan _periodLength;
    private readonly ImmutableArray<TTimedState> _states;

    private int _currentStateIndex;

    private TimeSpan _timePassedInPeriod = TimeSpan.Zero;

    public TimedOscillator(IEnumerable<TTimedState> states)
    {
        _states = states.ToImmutableArray();

        _periodLength = _states
            .Select(state => state.LastsFor)
            .Aggregate(TimeSpan.Zero, (t, s) => t + s);

        // TODO: Better implementation?
        var cumulativeDurations = _states
            .Select(state => state.LastsFor)
            .Aggregate(
                new[] {TimeSpan.Zero}.AsEnumerable(),
                (aggregateDurations, duration) =>
                {
                    var aggregatesEnumerated = aggregateDurations.ToImmutableArray();
                    return aggregatesEnumerated.Append(aggregatesEnumerated.Last() + duration);
                })
            .ToImmutableArray();

        _indexMap = cumulativeDurations
            .Zip(cumulativeDurations.Skip(1))
            .Select((tuple, i) => (Start: tuple.First, End: tuple.Second, Index: i))
            .ToImmutableDictionary(tuple => (tuple.Start, tuple.End), tuple => tuple.Index);
    }

    private TTimedState CurrentState => _states[_currentStateIndex];

    // TODO: How do we call the time deltas?
    public TTimedState GetCurrentState(TimeSpan elapsedTimeSinceLastFrame)
    {
        var periods = (int) (elapsedTimeSinceLastFrame / _periodLength);

        _timePassedInPeriod += elapsedTimeSinceLastFrame - periods * _periodLength;

        if (_timePassedInPeriod >= _periodLength)
            _timePassedInPeriod -= _periodLength;

        _currentStateIndex = _indexMap.First(tuple
            => tuple.Key.Start <= _timePassedInPeriod
               && _timePassedInPeriod < tuple.Key.End).Value;

        return CurrentState;
    }
}