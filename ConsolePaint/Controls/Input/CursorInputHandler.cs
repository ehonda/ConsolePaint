namespace ConsolePaint.Controls.Input;

// TODO: Clearer name! We handle input _for_ a cursor (not input _of_ a cursor)
public class CursorInputHandler : IInputHandler<ConsoleKey>
{
    private readonly Cursor _cursor;

    public CursorInputHandler(Cursor cursor)
    {
        _cursor = cursor;
    }
    
    public Task HandleInputAsync(ConsoleKey input)
    {
        Action action = input switch
        {
            ConsoleKey.UpArrow => () => _cursor.Move(Direction.Down),
            ConsoleKey.DownArrow => () => _cursor.Move(Direction.Up),
            ConsoleKey.RightArrow => () => _cursor.Move(Direction.Right),
            ConsoleKey.LeftArrow => () => _cursor.Move(Direction.Left),
            _ => () => {}
        };

        action();
        return Task.CompletedTask;
    }
}