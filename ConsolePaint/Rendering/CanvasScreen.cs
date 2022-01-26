using ConsolePaint.Demos;
using Spectre.Console;

namespace ConsolePaint.Rendering;

public class CanvasScreen : IScreen<Color>
{
    private readonly Color _background;
    private readonly Canvas _canvas;

    public CanvasScreen(int xLimit, int yLimit, Color background)
    {
        _background = background;
        _canvas = new(xLimit, yLimit);
    }

    public void DrawNewFrame()
    {
        foreach (var (x, y) in Generate.FlattenedGrid(_canvas.Width, _canvas.Height))
            _canvas.SetPixel(x, y, _background);
    }

    public void Draw(int x, int y, Color input)
    {
        _canvas.SetPixel(x, y, input);
    }

    public void Render()
    {
        AnsiConsole.Cursor.SetPosition(0, 0);
        AnsiConsole.Write(new Panel(_canvas).RoundedBorder().Header("Game"));
    }
}