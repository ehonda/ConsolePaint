using System.Collections.Immutable;
using ConsolePaint.Math;
using Spectre.Console;

namespace ConsolePaint.Demos;

public class OscillatingPixelsDemo
{
    private readonly Canvas _canvas = new(3, 3);

    private readonly ImmutableArray<OscillatingPixel> _pixels;

    private bool _running = true;

    public OscillatingPixelsDemo()
    {
        var states = ImmutableArray.Create<ColoredTimedState>(
            new()
            {
                Color = Color.Black,
                LastsFor = TimeSpan.FromMilliseconds(500)
            },
            new()
            {
                Color = Color.White,
                LastsFor = TimeSpan.FromMilliseconds(500)
            });

        var statesInverted = states.Reverse().ToImmutableArray();

        _pixels = Generate
            .FlattenedGrid(_canvas.Width, _canvas.Height)
            .Select(tuple => new OscillatingPixel(
                tuple.X,
                tuple.Y,
                (tuple.X + tuple.Y) % 2 == 0
                    ? states
                    : statesInverted))
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