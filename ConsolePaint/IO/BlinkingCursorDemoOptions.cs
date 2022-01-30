using CommandLine;
using JetBrains.Annotations;

namespace ConsolePaint.IO;

[UsedImplicitly]
[Verb("blinking-cursor", HelpText = "Run the blinking cursor demo")]
public record BlinkingCursorDemoOptions
{
    [Option('w', "width", Default = 40)]
    public int Width { get; init; }
    
    [Option('h', "height", Default = 20)]
    public int Height { get; init; }
    
    // TODO: Represent as timespan directly?
    [Option("off-ms", Default = 250)]
    public int OffForMilliseconds { get; init; }

    [Option("on-ms", Default = 1000)]
    public int OnForMilliseconds { get; init; }

    [Value(0, Default = BlinkingCursorDemoType.Plain)]
    public BlinkingCursorDemoType Type { get; init; }
}