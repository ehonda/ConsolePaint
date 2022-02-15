using System.Diagnostics;
using ConsolePaint.Controls.Input;
using Spectre.Console;

namespace ConsolePaint.Controls.GameLoops;

public abstract class GameLoop : IGameLoop
{
    private readonly IAnsiConsole _console;
    private readonly IInputReader<IAnsiConsole, ConsoleKeyInfo> _inputReader;
    private readonly IInputHandler<ConsoleKeyInfo> _inputHandler;
    private readonly IGameState _gameState;
    private readonly Stopwatch _stopwatch = new();

    // TODO: Inject interface for stopwatch
    protected GameLoop(
        IAnsiConsole console,
        IInputReader<IAnsiConsole, ConsoleKeyInfo> inputReader,
        IInputHandler<ConsoleKeyInfo> inputHandler,
        IGameState gameState)
    {
        _console = console;
        _inputReader = inputReader;
        _inputHandler = inputHandler;
        _gameState = gameState;
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
        while (_gameState.Running)
        {
            await ProcessInputAsync();
            await RenderAsync(_stopwatch.Elapsed);
            await Task.Delay(TimeSpan.FromMilliseconds(1));
        }
        _stopwatch.Stop();
    }

    private async Task ProcessInputAsync()
    {
        var (success, input) = await _inputReader.TryReadInputAsync(_console);
        if (success)
            await _inputHandler.HandleInputAsync(input);
    }

    protected abstract Task RenderAsync(TimeSpan currentTime);
}