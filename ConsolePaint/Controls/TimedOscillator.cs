using System.Collections.Immutable;
using ConsolePaint.Extensions;

namespace ConsolePaint.Controls;

// TODO: States immutable, oscillator tracks elapsed? We don't want to be able to initialize
// TODO: with states that have elapsed time, nor do we want the outside to be able to advance time in state 
public class TimedOscillator
{
    private readonly ImmutableArray<TimedState> _states;
    // TODO: Use cyclic enumerable enumerator
    private int _currentStateIndex;
    private TimedState CurrentState => _states[_currentStateIndex];
    
    public TimedOscillator(IEnumerable<TimedState> states)
    {
        _states = states.ToImmutableArray();
    }
    
    // TODO: How do we call the time deltas?
    public TimedState GetCurrentState(TimeSpan elapsedTimeSinceLastFrame)
    {
        // TODO: Can we calculate this more elegantly?
        var elapsedTimeSinceEndOfLastFrameState = CurrentState.Elapsed
            + elapsedTimeSinceLastFrame - CurrentState.LastsFor;

        if (elapsedTimeSinceEndOfLastFrameState > TimeSpan.Zero)
        {
            _currentStateIndex = _currentStateIndex.AddModulo(1, _states.Length);
            CurrentState.Elapsed = elapsedTimeSinceEndOfLastFrameState;
        }
        else
        {
            CurrentState.Elapsed += elapsedTimeSinceLastFrame;
        }

        return CurrentState;
    }
}