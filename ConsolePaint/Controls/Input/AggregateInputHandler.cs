using System.Collections.Immutable;

namespace ConsolePaint.Controls.Input;

public class AggregateInputHandler : IInputHandler<ConsoleKey>
{
    private readonly ImmutableArray<IInputHandler<ConsoleKey>> _handlers;

    public AggregateInputHandler(IEnumerable<IInputHandler<ConsoleKey>> handlers)
    {
        _handlers = handlers.ToImmutableArray();
    }
    
    public async Task HandleInputAsync(ConsoleKey input)
    {
        foreach (var handler in _handlers)
        {
            await handler.HandleInputAsync(input);
        }
    }
}