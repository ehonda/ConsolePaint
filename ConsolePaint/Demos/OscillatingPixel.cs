using System.Collections.Immutable;
using ConsolePaint.Controls;
using Spectre.Console;

namespace ConsolePaint.Demos;

public class OscillatingPixel : IRenderable
{
    private readonly int _x;
    private readonly int _y;
    private readonly TimedOscillator<ColoredTimedState> _oscillator;
    
    public OscillatingPixel(int x, int y, params ColoredTimedState[] states)
        : this(x, y, states.ToImmutableArray())
    {
    }
    
    public OscillatingPixel(int x, int y, IEnumerable<ColoredTimedState> states)
    {
        _x = x;
        _y = y;
        _oscillator = new(states);
    }

    public void Render(Canvas canvas, TimeSpan elapsedTimeSinceLastFrame)
    {
        canvas.SetPixel(_x, _y, _oscillator.Step(elapsedTimeSinceLastFrame).Color);
    }
}