using CommandLine;
using JetBrains.Annotations;

namespace ConsolePaint.IO;

[UsedImplicitly]
[Verb("oscillating-pixels", HelpText = "Run the oscillating pixels demo")]
public record OscillatingPixelsDemoOptions;