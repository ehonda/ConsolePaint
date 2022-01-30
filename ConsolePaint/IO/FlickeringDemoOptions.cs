using CommandLine;
using JetBrains.Annotations;

namespace ConsolePaint.IO;

[UsedImplicitly]
[Verb("flickering", HelpText = "Run the flickering demo")]
public record FlickeringDemoOptions
{
    [Option('f', "flicker", Default = false)]
    public bool Flicker { get; init; }
}