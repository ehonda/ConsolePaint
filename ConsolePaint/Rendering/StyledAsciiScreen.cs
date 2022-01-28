using ConsolePaint.Demos;
using Spectre.Console;

namespace ConsolePaint.Rendering;

public class StyledAsciiScreen : IScreen<(char Character, Style Style)>
{
    private readonly int _xLimit;
    private readonly int _yLimit;
    
    public Color Background { get; }

    // TODO: Better name
    private readonly IDictionary<(int X, int Y), (char Character, Style Style)> _screen;

    public StyledAsciiScreen(int xLimit, int yLimit, Color background)
    {
        _xLimit = xLimit;
        _yLimit = yLimit;
        Background = background;
        _screen = new Dictionary<(int X, int Y), (char Character, Style Style)>();
    }
    
    public void DrawNewFrame()
    {
        // TODO: Do we want / need this background clearing functionality?
        _screen.Clear();
        foreach (var position in Generate.FlattenedGrid(_xLimit, _yLimit))
            _screen[position] = (' ', new(background: Background));
    }

    public void Draw(int x, int y, (char Character, Style Style) input)
    {
        _screen[(x, y)] = input;
    }

    public void Render()
    {
        var renderable = _screen
            .Aggregate(
                new Paragraph(),
                (paragraph, tuple) =>
                {
                    paragraph.Append($"{tuple.Value.Character}", tuple.Value.Style);
                    
                    if (tuple.Key.X == _xLimit - 1 && tuple.Key.Y != _yLimit - 1)
                        paragraph.Append(Environment.NewLine);

                    return paragraph;
                });
        
        AnsiConsole.Cursor.SetPosition(0, 0);
        AnsiConsole.Write(new Panel(renderable).RoundedBorder().Header("Game"));
    }
}