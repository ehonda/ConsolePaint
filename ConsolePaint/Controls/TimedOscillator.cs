using System.Collections.Immutable;
using ConsolePaint.Math;

namespace ConsolePaint.Controls;

public class TimedOscillator<TTimedState> where TTimedState : TimedState
{
    private readonly ICoveringByDisjointIntervals<TimeSpan> _covering;

    private readonly ImmutableArray<TTimedState> _states;

    private int _currentStateIndex;

    // TODO: Do we really want / need to save time passed since beginning?
    private TimeSpan _timePassed = TimeSpan.Zero;

    // TODO:
    public TTimedState CurrentState => _states[_currentStateIndex];

    public TimedOscillator(params TTimedState[] states)
        : this(states.ToImmutableArray())
    {
    }

    public TimedOscillator(IEnumerable<TTimedState> states)
    {
        _states = states.ToImmutableArray();

        var covering = CoveringByDisjointIntervals<TimeSpan>
            .FromIntervalLengths(
                TimeSpan.Zero,
                (t, s) => t + s,
                _states.Select(state => state.LastsFor));

        _covering = new PeriodicCoveringByDisjointIntervals<TimeSpan>(
            covering,
            (t, s) => t - s,
            (t, s) => t / s,
            (d, t) => d * t);
    }
    
    // TODO: Do we want to pass time since simulation started instead?
    
    // TODO: The way this is implemented, it has the visible side effect of "advancing time"
    // TODO: when called -> Bad if we want to use the result twice -> argument for passing time since beginning
    public TTimedState Step(TimeSpan elapsedTimeSinceLastFrame)
    {
        _timePassed += elapsedTimeSinceLastFrame;

        _currentStateIndex = _covering.GetCover(_timePassed).Index;

        return CurrentState;
    }
}