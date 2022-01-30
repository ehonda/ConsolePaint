using System.Collections.Immutable;
using ConsolePaint.Math;
using ConsolePaint.Rendering;
using Spectre.Console;

namespace ConsolePaint.Demos;

public class OscillatingPixelsDemo
{
    private const int Size = 3;
    
    private readonly IScreen<Color> _screen;

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
            .FlattenedGrid(Size, Size)
            .Select(tuple => new OscillatingPixel(
                tuple.X,
                tuple.Y,
                (tuple.X + tuple.Y) % 2 == 0
                    ? states
                    : statesInverted))
            .ToImmutableArray();

        _screen = new CanvasScreen(Size, Size, Color.Black);
    }

    public async Task RunAsync()
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
        _screen.DrawNewFrame();
        DrawPixels(elapsed);
        _screen.Render();
        return Task.CompletedTask;
    }

    private void DrawPixels(TimeSpan elapsed)
    {
        foreach (var pixel in _pixels) pixel.Render(_screen, elapsed);
    }
}