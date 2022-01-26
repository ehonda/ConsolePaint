using Spectre.Console;

namespace ConsolePaint.Rendering;

// TODO: Rename to not clash with Spectre.Console interface?
// TODO: To better namespace
public interface IRenderable
{
    // TODO: Use something else than Canvas, e.g. Screen class here
    // TODO: Pass time delta or time since beginning?
    public void Render(Canvas canvas, TimeSpan elapsedTimeSinceLastFrame);
}