using ConsolePaint.Demos;
using Spectre.Console;

namespace ConsolePaint.Rendering;

public class TextScreen : IScreen<Text>
{
    public Color Background { get; }
    
    private readonly Canvas _canvas;

    // TODO: Is "Texture" a fitting name here?
    private readonly Queue<(int X, int Y, Text Texture)> _textures = new();

    public TextScreen(int xLimit, int yLimit, Color background)
    {
        Background = background;
        _canvas = new(xLimit, yLimit);
    }

    public void DrawNewFrame()
    {
        _textures.Clear();
        foreach (var (x, y) in Generate.FlattenedGrid(_canvas.Width, _canvas.Height))
            _canvas.SetPixel(x, y, Background);
    }

    public void Draw(int x, int y, Text input)
    {
        _textures.Enqueue((x, y, input));
    }

    public void Render()
    {
        AnsiConsole.Cursor.SetPosition(0, 0);
        AnsiConsole.Write(_canvas);
        foreach (var (x, y, texture) in _textures)
        {
            AnsiConsole.Cursor.SetPosition(x, y);
            AnsiConsole.Write(texture);
        }

        for (int i = 0; i < 20; ++i)
        {
            AnsiConsole.Cursor.SetPosition(5, i);
            AnsiConsole.Write(new Text("$", new(Color.Black, Background)));
        }
        
        for (int i = 0; i < 20; ++i)
        {
            AnsiConsole.Cursor.SetPosition(i, 5);
            AnsiConsole.Write(new Text("$", new(Color.Black, Background)));
        }
        
        for (int i = 0; i < 20; ++i)
        {
            AnsiConsole.Cursor.SetPosition(6, i);
            AnsiConsole.Write(new Text(" ", new(Color.Black, Color.Black)));
        }
        
        for (int i = 0; i < 20; ++i)
        {
            AnsiConsole.Cursor.SetPosition(i, 6);
            AnsiConsole.Write(new Text(" ", new(Color.Black, Color.Black)));
        }
    }
}