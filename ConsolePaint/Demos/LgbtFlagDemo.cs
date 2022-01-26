using System.Collections.Immutable;
using Spectre.Console;

namespace ConsolePaint.Demos;

public class LgbtFlagDemo
{
    private readonly Canvas _canvas = new(12, 6);

    private readonly ImmutableArray<OscillatingPixel> _pixels;

    private bool _running = true;

    public LgbtFlagDemo(TimeSpan? delay = null)
    {
        var states = new Color[]
            {
                // Scheme taken from https://www.schemecolor.com/lgbt-flag-colors.php
                new(255, 0, 24),
                new(255, 165, 44),
                new(255, 255, 65),
                new(0, 128, 24),
                new(0, 0, 249),
                new(134, 0, 125)
            }
            .Select(color => new ColoredTimedState
            {
                Color = color,
                LastsFor = delay ?? TimeSpan.FromMilliseconds(500)
            })
            .ToImmutableArray();

        IEnumerable<T> LeftShiftCyclically<T>(ImmutableArray<T> xs, int shift)
            => xs.Skip(xs.Length - shift).Concat(xs.Take(xs.Length - shift));
        
        _pixels = Enumerable
            .Repeat(Enumerable.Range(0, _canvas.Width), _canvas.Height)
            .Zip(Enumerable.Range(0, _canvas.Height))
            .SelectMany(tuple => tuple.First.Select(x => (X: x, Y: tuple.Second)))
            .Select(tuple => new OscillatingPixel(
                tuple.X,
                tuple.Y,
                // states))
                LeftShiftCyclically(states, tuple.Y)))
            .ToImmutableArray();
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
            }
        }
    }

    private Task Render(TimeSpan elapsed)
    {
        AnsiConsole.Cursor.SetPosition(0, 0);
        DrawBackground();
        DrawPixels(elapsed);
        AnsiConsole.Write(new Panel(_canvas).RoundedBorder().Header("Game"));
        return Task.CompletedTask;
    }

    private void DrawPixels(TimeSpan elapsed)
    {
        foreach (var pixel in _pixels) pixel.Render(_canvas, elapsed);
    }

    private void DrawBackground()
    {
        for (var x = 0; x < _canvas.Width; x++)
        for (var y = 0; y < _canvas.Height; y++)
            _canvas.SetPixel(x, y, Color.Black);
    }
}