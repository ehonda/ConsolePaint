using Spectre.Console;

namespace ConsolePaint.Controls.Input;

public class AnsiConsoleInputReader : IInputReader<IAnsiConsole, ConsoleKeyInfo>
{
    public async Task<(bool Success, ConsoleKeyInfo Input)> TryReadInputAsync(IAnsiConsole console)
    {
        // TODO: Replace by console.Input.KeyAvailable once released
        if (Console.KeyAvailable is false)
            return (false, default);

        var input = await console.Input.ReadKeyAsync(true, CancellationToken.None);
        
        // Should never happen since null is only returned on cancellation
        return input is null 
            ? (false, default)
            : (true, input.Value);
    }
}