using Ardalis.GuardClauses;
using ConsolePaint.Math.Extensions;

namespace ConsolePaint.Controls;

// TODO: Rename to CyclicCursor, make non-cyclic version?
public class Cursor
{
    public int X { get; private set; }
    public int Y { get; private set; }

    private readonly int _xLimit;
    private readonly int _yLimit;

    public Cursor(int x, int y, int xLimit, int yLimit)
    {
        Guard.Against.OutOfRange(xLimit, nameof(xLimit), 0, int.MaxValue);
        Guard.Against.OutOfRange(yLimit, nameof(yLimit), 0, int.MaxValue);
        Guard.Against.OutOfRange(x, nameof(x), 0, xLimit);
        Guard.Against.OutOfRange(y, nameof(y), 0, yLimit);
        
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
            Y = Y.AddModulo(step, _yLimit);
            return;
        }

        X = X.AddModulo(step, _xLimit);
    }
}