namespace ConsolePaint.Controls;

public class Cursor
{
    public int X { get; private set; }
    public int Y { get; private set; }

    private readonly int _xLimit;
    private readonly int _yLimit;

    public Cursor(int x, int y, int xLimit, int yLimit)
    {
        X = x;
        Y = y;
        _xLimit = xLimit;
        _yLimit = yLimit;
    }

    public Cursor(int xLimit, int yLimit)
        : this(0, 0, xLimit, yLimit)
    {
    }

    public void Move(Direction direction)
    {
        var step = direction switch
        {
            Direction.Up => 1,
            Direction.Down => -1,
            Direction.Right => 1,
            _ => -1
        };

        if (direction is Direction.Up or Direction.Down)
        {
            Y = AddModular(Y, step, _yLimit);
            return;
        }

        X = AddModular(X, step, _xLimit);
    }

    private int AddModular(int a, int b, int n)
        => (a + b + n) % n;
}