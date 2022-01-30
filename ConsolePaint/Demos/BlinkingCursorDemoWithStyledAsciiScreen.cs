﻿using ConsolePaint.Controls;
using ConsolePaint.Rendering;
using Spectre.Console;

namespace ConsolePaint.Demos;

public class BlinkingCursorDemoWithStyledAsciiScreen : IBlinkingCursorDemo
{
    private readonly int _xLimit;
    private readonly int _yLimit;
    private readonly BlinkingCursor _cursor;
    private readonly IScreen<(char Character, Style Style)> _screen;
    
    private readonly Color _background = Color.Gold3;
    
    private bool _running = true;
    
    public BlinkingCursorDemoWithStyledAsciiScreen(
        int xLimit, int yLimit, TimeSpan offFor, TimeSpan onFor)
    {
        _xLimit = xLimit;
        _yLimit = yLimit;
        _cursor = new(xLimit, yLimit, offFor, onFor, Color.Red3);
        _screen = new StyledAsciiScreen(xLimit, yLimit, _background);
    }

    public async Task RunAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Cursor.Hide();
        
        // TODO: Use stopwatch
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

        AnsiConsole.Clear();
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
        _screen.DrawNewFrame();
        RenderBushes();
        RenderCursor(elapsed);
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

    private void RenderCursor(TimeSpan elapsed)
    {
        _cursor.Render(_screen, elapsed);
    }
}