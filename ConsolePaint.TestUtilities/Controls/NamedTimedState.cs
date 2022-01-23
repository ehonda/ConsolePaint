using ConsolePaint.Controls;
using JetBrains.Annotations;

namespace ConsolePaint.TestUtilities.Controls;

[PublicAPI]
public class NamedTimedState : TimedState
{
    public string Name { get; init; } = string.Empty;
}