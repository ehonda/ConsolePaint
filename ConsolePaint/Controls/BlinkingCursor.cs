using ConsolePaint.Rendering;
using Spectre.Console;

namespace ConsolePaint.Controls;

public class BlinkingCursor : Cursor, IRenderable,
    IRenderableToScreen<Color>, IRenderableToScreen<Text>
{
    private readonly Color _onColor;
    private readonly TimedOscillator<EnumValuedTimedState<BlinkingCursorState>> _oscillator;

    // TODO: Get rid of this mass of parameters!
    public BlinkingCursor(int x, int y, int xLimit, int yLimit,
        TimeSpan offFor, TimeSpan onFor, Color onColor)
        : base(x, y, xLimit, yLimit)
    {
        _onColor = onColor;
        _oscillator = new(
            new(offFor, BlinkingCursorState.Off),
            new(onFor, BlinkingCursorState.On));
    }

    public BlinkingCursor(int xLimit, int yLimit,
        TimeSpan offFor, TimeSpan onFor, Color onColor)
        : this(0, 0, xLimit, yLimit, offFor, onFor, onColor)
    {
    }

    public void Render(Canvas canvas, TimeSpan elapsedTimeSinceLastFrame)
    {
        if (_oscillator.Step(elapsedTimeSinceLastFrame).Value is BlinkingCursorState.On)
            canvas.SetPixel(X, Y, _onColor);
    }

    public void Render(IScreen<Color> screen, TimeSpan elapsedTimeSinceLastFrame)
    {
        if (_oscillator.Step(elapsedTimeSinceLastFrame).Value is BlinkingCursorState.On)
            screen.Draw(X, Y, _onColor);
    }

    public void Render(IScreen<Text> screen, TimeSpan elapsedTimeSinceLastFrame)
    {
        if (_oscillator.Step(elapsedTimeSinceLastFrame).Value is BlinkingCursorState.On)
            screen.Draw(X, Y, new("@", new(_onColor)));
    }
}