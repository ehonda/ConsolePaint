using ConsolePaint.Controls;
using ConsolePaint.Math;
using Spectre.Console;

namespace ConsolePaint.Demos;

public class FlickeringDemo
{
    private bool _running = true;
    
    private readonly Color _background = Color.DarkGoldenrod;
    private readonly Color _foreground = Color.Black;
    private readonly Style _style;
    private readonly TimedOscillator<EnumValuedTimedState<BlinkingCursorState>> _oscillator;

    private const int N = 20;

    public FlickeringDemo()
    {
        _style = new(_foreground, _background);
        _oscillator = new(Enum
            .GetValues<BlinkingCursorState>()
            .Select(value => new EnumValuedTimedState<BlinkingCursorState>(
                TimeSpan.FromMilliseconds(250), value)));

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
            }
        }
    }

    private Task Render(TimeSpan elapsed)
    {
        foreach (var (x, y) in Generate.FlattenedGrid(N, N))
            WriteText(x, y, " ");

        if (_oscillator.Step(elapsed).Value is BlinkingCursorState.On)
        {
            for (int i = 0; i < N; i++)
            {
                WriteText(i, 9, "$");
                WriteText(i, 10, "$");
            }
            return Task.CompletedTask;
        }
        
        for (int i = 0; i < N; i++)
        {
            WriteText(9, i, "$");
            WriteText(10, i, "$");
        }
        return Task.CompletedTask;
    }
    
    private void WriteText(int x, int y, string text)
    {
        AnsiConsole.Cursor.SetPosition(x, y);
        AnsiConsole.Write(new Text(text, _style));
    }
}