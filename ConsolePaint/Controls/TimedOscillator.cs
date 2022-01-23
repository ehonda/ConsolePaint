using System.Collections.Immutable;
using ConsolePaint.Math;

namespace ConsolePaint.Controls;

// TODO: States immutable, oscillator tracks elapsed? We don't want to be able to initialize
// TODO: with states that have elapsed time, nor do we want the outside to be able to advance time in state 
public class TimedOscillator<TTimedState> where TTimedState : TimedState
{
    private readonly CoveringByDisjointIntervals<TimeSpan> _covering;

    private readonly TimeSpan _periodLength;
    private readonly ImmutableArray<TTimedState> _states;

    private int _currentStateIndex;

    private TimeSpan _timePassedInPeriod = TimeSpan.Zero;

    public TimedOscillator(params TTimedState[] states)
        : this(states.ToImmutableArray())
    {
    }

    public TimedOscillator(IEnumerable<TTimedState> states)
    {
        _states = states.ToImmutableArray();
        
        _covering = CoveringByDisjointIntervals<TimeSpan>
            .FromIntervalLengths(
                TimeSpan.Zero,
                (t, s) => t + s,
                _states.Select(state => state.LastsFor));

        _periodLength = _covering.End;
    }

    private TTimedState CurrentState => _states[_currentStateIndex];

    // TODO: How do we call the time deltas?
    public TTimedState GetCurrentState(TimeSpan elapsedTimeSinceLastFrame)
    {
        var periods = (int) (elapsedTimeSinceLastFrame / _periodLength);

        _timePassedInPeriod += elapsedTimeSinceLastFrame - periods * _periodLength;

        if (_timePassedInPeriod >= _periodLength)
            _timePassedInPeriod -= _periodLength;
        
        _currentStateIndex = _covering.GetCover(_timePassedInPeriod).Index;

        return CurrentState;
    }
}