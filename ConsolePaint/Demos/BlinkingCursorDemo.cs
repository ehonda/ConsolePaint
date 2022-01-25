using ConsolePaint.Controls;
using Spectre.Console;

namespace ConsolePaint.Demos;

// TODO: Add input queue
public class BlinkingCursorDemo
{
    private readonly BlinkingCursor _cursor;
    private readonly Canvas _canvas;
    
    private readonly Color _background = Color.Gold3;

    private bool _running = true;
    
    public BlinkingCursorDemo(int xLimit, int yLimit, TimeSpan offFor, TimeSpan onFor)
    {
        _cursor = new(xLimit, yLimit, offFor, onFor, Color.Red3);
        _canvas = new(xLimit, yLimit);
    }

    public async Task Run()
    {
        AnsiConsole.Clear();
        AnsiConsole.Cursor.Hide();
        
        var lastRender = DateTimeOffset.UtcNow;
        while (_running)
        {
            await ProcessInput();
            var currentRender = DateTimeOffset.UtcNow;
            await Render(currentRender - lastRender);
            lastRender = currentRender;
            // ReSharper disable once MethodSupportsCancellation
            await Task.Delay(TimeSpan.FromMilliseconds(1));
        }

        AnsiConsole.Cursor.Show();
        AnsiConsole.Reset();
    }

    private async Task ProcessInput()
    {
        // TODO: Replace by AnsiConsole.Console.Input.KeyAvailable once released
        if (Console.KeyAvailable)
        {
            var input = await AnsiConsole.Console.Input.ReadKeyAsync(true, CancellationToken.None);
            if (input is null)
                return;

            var key = input.Value.Key;
            switch (key)
            {
                case ConsoleKey.Escape:
                    _running = false;
                    break;
                
                case ConsoleKey.UpArrow:
                    _cursor.Move(Direction.Down);
                    break;
                
                case ConsoleKey.DownArrow:
                    _cursor.Move(Direction.Up);
                    break;
                
                case ConsoleKey.RightArrow:
                    _cursor.Move(Direction.Right);
                    break;
                
                case ConsoleKey.LeftArrow:
                    _cursor.Move(Direction.Left);
                    break;
            }
        }
    }

    private Task Render(TimeSpan elapsed)
    {
        AnsiConsole.Cursor.SetPosition(0, 0);
        DrawBackground();
        DrawCursor(elapsed);
        AnsiConsole.Write(new Panel(_canvas).RoundedBorder().Header("Game"));
        AnsiConsole.Write(new Panel("Command").RoundedBorder().Header("Console"));
        return Task.CompletedTask;
    }

    private void DrawCursor(TimeSpan elapsed)
    {
        _cursor.Render(_canvas, elapsed);
    }
    
    private void DrawBackground()
    {
        for (int x = 0; x < _canvas.Width; x++)
        {
            for (int y = 0; y < _canvas.Height; y++)
            {
                _canvas.SetPixel(x, y, _background);
            }
        }
    }

}