using ConsolePaint.Controls;
using ConsolePaint.Controls.GameLoops;
using ConsolePaint.Controls.Input;
using ConsolePaint.Rendering;
using Spectre.Console;

namespace ConsolePaint.Demos;

public class GameLoopDemo : GameLoop, IBlinkingCursorDemo
{
    private readonly int _xLimit;
    private readonly int _yLimit;
    private readonly BlinkingCursor _cursor;
    private readonly IScreen<(char Character, Style Style)> _screen;
    private readonly Color _background = Color.Gold3;

    public static GameLoopDemo Create(
        int xLimit, int yLimit, TimeSpan offFor, TimeSpan onFor)
    {
        var gameState = new GameState { Running = true };
        var cursor = new BlinkingCursor(xLimit, yLimit, offFor, onFor, Color.Red3);
        
        var inputHandler = new TransformInputHandler<ConsoleKeyInfo, ConsoleKey>(
            info => info.Key,
            new AggregateInputHandler(new IInputHandler<ConsoleKey>[]
            {
                new EscapeInputHandler(() => gameState.Running = false),
                new CursorInputHandler(cursor)
            }));

        var console = AnsiConsole.Console;

        return new(
            xLimit, yLimit, cursor,
            console, new AnsiConsoleInputReader(), inputHandler,
            gameState);
    }
    
    private GameLoopDemo(
        int xLimit, int yLimit, BlinkingCursor cursor,
        IAnsiConsole console,
        IInputReader<IAnsiConsole, ConsoleKeyInfo> inputReader,
        IInputHandler<ConsoleKeyInfo> inputHandler,
        IGameState gameState)
        : base(console, inputReader, inputHandler, gameState)
    {
        _xLimit = xLimit;
        _yLimit = yLimit;
        _cursor = cursor;
        _screen = new StyledAsciiScreen(xLimit, yLimit, _background);
    }

    protected override Task RenderAsync(TimeSpan currentTime)
    {
        _screen.DrawNewFrame();
        RenderBushes();
        RenderCursor(currentTime);
        _screen.Render();
        return Task.CompletedTask;
    }
    
    private void RenderBushes()
    {
        var y = _yLimit / 2;

        for (var x = 0; x < _xLimit; x++)
        {
            if (x % 2 == 0)
            {
                _screen.Draw(x, y, ('$', new(Color.Black, _background, Decoration.Underline | Decoration.Bold)));
                continue;
            }
            
            _screen.Draw(x, y, ('$', new(Color.Black, _background, Decoration.Strikethrough)));
        }
    }

    private void RenderCursor(TimeSpan currentTime)
    {
        _cursor.RenderWithCurrentTime(_screen, currentTime);
    }
}