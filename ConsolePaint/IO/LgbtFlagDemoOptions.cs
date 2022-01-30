using CommandLine;
using JetBrains.Annotations;

namespace ConsolePaint.IO;

[UsedImplicitly]
[Verb("lgbt-flag", HelpText = "Run the lgbt flag demo")]
public record LgbtFlagDemoOptions
{
    [Option('d', "delay-ms", Default = 500)]
    public int DelayMilliseconds { get; init; }
}