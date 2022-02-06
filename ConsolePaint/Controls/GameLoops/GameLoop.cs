using System.Diagnostics;
using Spectre.Console;

namespace ConsolePaint.Controls.GameLoops;

public abstract class GameLoop : IGameLoop
{
    private readonly IAnsiConsole _console;
    private readonly Stopwatch _stopwatch = new();

    private bool _running = true;
    
    // TODO: Inject interface for stopwatch
    protected GameLoop(IAnsiConsole console)
    {
        _console = console;
    }
    
    public async Task RunAsync()
    {
        _console.Clear();
        _console.Cursor.Hide();

        await RunGameLoopAsync();
        
        _console.Cursor.Show();
    }

    private async Task RunGameLoopAsync()
    {
        _stopwatch.Start();
        while (_running)
        {
            await ProcessInputAsync();
            await RenderAsync(_stopwatch.Elapsed);
            await Task.Delay(TimeSpan.FromMilliseconds(1));
        }
        _stopwatch.Stop();
    }

    // TODO: Implement reading key from console
    protected abstract Task ProcessInputAsync();
    
    protected abstract Task RenderAsync(TimeSpan currentTime);
}