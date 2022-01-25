using ConsolePaint.Controls;
using Spectre.Console;

namespace ConsolePaint.Demos;

public class ColoredTimedState : TimedState
{
    public Color Color { get; init; }
}