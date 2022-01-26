using ConsolePaint.Demos;
using Spectre.Console;

namespace ConsolePaint.Rendering;

public class TextScreen : IScreen<Text>
{
    private readonly Color _background;
    private readonly Canvas _canvas;

    public TextScreen(int xLimit, int yLimit, Color background)
    {
        _background = background;
        _canvas = new(xLimit, yLimit);
    }

    public void DrawNewFrame()
    {
        foreach (var (x, y) in Generate.FlattenedGrid(_canvas.Width, _canvas.Height))
            _canvas.SetPixel(x, y, _background);
    }

    public void Draw(int x, int y, Text input)
    {
        
    }

    public void Render()
    {
        throw new NotImplementedException();
    }
}