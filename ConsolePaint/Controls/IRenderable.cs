using Spectre.Console;

namespace ConsolePaint.Controls;

// TODO: Rename to not clash with Spectre.Console interface?
// TODO: To better namespace
public interface IRenderable
{
    // TODO: Use something else than Canvas, e.g. Screen class here
    public void Render(Canvas canvas, TimeSpan elapsedTimeSinceLastFrame);
}