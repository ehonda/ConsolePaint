using ConsolePaint.Controls;
using Spectre.Console;

namespace ConsolePaint.Demos;

public class NonFlickeringDemo
{
    private bool _running = true;
    
    private readonly Color _background = Color.DarkGoldenrod;
    private readonly Color _foreground = Color.Black;
    private readonly Style _style;
    private readonly TimedOscillator<EnumValuedTimedState<BlinkingCursorState>> _oscillator;

    private const int N = 20;

    private readonly char[][] _characters;
    
    public NonFlickeringDemo()
    {
        _style = new(_foreground, _background);
        _oscillator = new(Enum
            .GetValues<BlinkingCursorState>()
            .Select(value => new EnumValuedTimedState<BlinkingCursorState>(
                TimeSpan.FromMilliseconds(250), value)));
                
        _characters = Enumerable
            .Repeat(Enumerable.Repeat(' ', N), N)
            .Select(row => row.ToArray())
            .ToArray();
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
            SetCharacter(x, y, ' ');

        if (_oscillator.Step(elapsed).Value is BlinkingCursorState.On)
        {
            for (int i = 0; i < N; i++)
            {
                SetCharacter(i, 9, '$');
                SetCharacter(i, 10, '$');
            }
        }
        else
        {
            for (int i = 0; i < N; i++)
            {
                SetCharacter(9, i, '$');
                SetCharacter(10, i, '$');
            }
        }

        AnsiConsole.Cursor.SetPosition(0, 0);
        AnsiConsole.Write(CreateTextToRender());

        return Task.CompletedTask;
    }
    
    private void SetCharacter(int x, int y, char text)
    {
        _characters[y][x] = text;
    }

    private Text CreateTextToRender()
    {
        var stringRows = _characters
            .Select(row => new string(row));

        var rowsJoined = string.Join(Environment.NewLine, stringRows);

        return new(rowsJoined, _style);
    }
}