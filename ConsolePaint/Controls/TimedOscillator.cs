using System.Collections.Immutable;
using ConsolePaint.Math;
using ConsolePaint.Utility;

namespace ConsolePaint.Controls;

public class TimedOscillator<TTimedState> where TTimedState : TimedState
{
    private readonly ICoveringByDisjointIntervals<TimeSpan> _covering;

    private readonly ImmutableArray<TTimedState> _states;

    // TODO: Remove
    private int _currentStateIndex;

    // TODO: Do we really want / need to save time passed since beginning?
    private TimeSpan _timePassed = TimeSpan.Zero;

    // TODO: Remove
    public TTimedState CurrentState => _states[_currentStateIndex];

    public TimedOscillator(params TTimedState[] states)
        // ReSharper disable once ConstantConditionalAccessQualifier
        : this(states?.ToImmutableArray()!)
    {
    }

    // TODO: Test guarding against null / empty states
    public TimedOscillator(IEnumerable<TTimedState> states)
        => (_states, _covering) = Enumerate.AndApplyGuardingAgainstNullOrEmpty(
            states,
            // ReSharper disable once VariableHidesOuterVariable
            states =>
            {
                var covering = CoveringByDisjointIntervals.FromIntervalLengths(
                    TimeSpan.Zero,
                    (t, s) => t + s,
                    states.Select(state => state.LastsFor));

                var periodicCovering = new PeriodicCoveringByDisjointIntervals<TimeSpan>(
                    covering,
                    (t, s) => t - s,
                    (t, s) => t / s,
                    (d, t) => d * t);

                return (states, periodicCovering);
            });
    
    // TODO: Do we want to pass time since simulation started instead?
    
    // TODO: The way this is implemented, it has the visible side effect of "advancing time"
    // TODO: when called -> Bad if we want to use the result twice -> argument for passing time since beginning
    public TTimedState Step(TimeSpan elapsedTimeSinceLastFrame)
    {
        _timePassed += elapsedTimeSinceLastFrame;

        _currentStateIndex = _covering.GetCover(_timePassed).Index;

        return CurrentState;
    }

    public TTimedState StateAt(TimeSpan time)
        => _states[_covering.GetCover(time).Index];
}