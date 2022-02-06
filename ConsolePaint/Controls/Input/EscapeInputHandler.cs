namespace ConsolePaint.Controls.Input;

// TODO: Better name!
public class EscapeInputHandler : IInputHandler<ConsoleKey>
{
    private readonly Action _onEscapeInput;

    public EscapeInputHandler(Action onEscapeInput)
    {
        _onEscapeInput = onEscapeInput;
    }
    
    public Task HandleInputAsync(ConsoleKey input)
    {
        if (input is ConsoleKey.Escape)
            _onEscapeInput();
        
        return Task.CompletedTask;
    }
}