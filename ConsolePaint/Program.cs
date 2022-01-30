using CommandLine;
using ConsolePaint.Demos;
using ConsolePaint.IO;

var parser = new Parser(settings =>
{
    settings.CaseInsensitiveEnumValues = true;
});

// TODO: Usage of command line parser with async does not work great here
//          - WithParsedAsync not chainable so we need MapResult
//          - Help is not displayed, exceptions are swallowed
//          - How do we properly use this? https://github.com/commandlineparser/commandline/wiki
await parser
    .ParseArguments<
        BlinkingCursorDemoOptions,
        OscillatingPixelsDemoOptions,
        LgbtFlagDemoOptions,
        FlickeringDemoOptions>(args)
    .MapResult(
        async (BlinkingCursorDemoOptions options) => await RunBlinkingCursorDemoAsync(options),
        async (OscillatingPixelsDemoOptions options) => await RunOscillatingPixelsDemoAsync(options),
        async (LgbtFlagDemoOptions options) => await RunLgbtFlagDemoAsync(options),
        async (FlickeringDemoOptions options) => await RunFlickeringDemoAsync(options),
        async _ => await Task.CompletedTask);

async Task RunBlinkingCursorDemoAsync(BlinkingCursorDemoOptions options)
{
    var w = options.Width;
    var h = options.Height;
    var offFor = TimeSpan.FromMilliseconds(options.OffForMilliseconds);
    var onFor = TimeSpan.FromMilliseconds(options.OnForMilliseconds);

    IBlinkingCursorDemo demo = options.Type switch
    {
        BlinkingCursorDemoType.Plain => new BlinkingCursorDemo(
            w, h, offFor, onFor),

        BlinkingCursorDemoType.Color => new BlinkingCursorDemoWithColorScreen(
            w, h, offFor, onFor),

        BlinkingCursorDemoType.Text => new BlinkingCursorDemoWithTextScreen(
            w, h, offFor, onFor),

        BlinkingCursorDemoType.Ascii => new BlinkingCursorDemoWithStyledAsciiScreen(
            w, h, offFor, onFor),

        // TODO: Better exception
        _ => throw new ArgumentException(nameof(options.Type))
    };

    await demo.RunAsync();
}

// ReSharper disable once UnusedParameter.Local
async Task RunOscillatingPixelsDemoAsync(OscillatingPixelsDemoOptions _)
{
    var demo = new OscillatingPixelsDemo();
    await demo.RunAsync();
}

async Task RunLgbtFlagDemoAsync(LgbtFlagDemoOptions options)
{
    var demo = new LgbtFlagDemo(TimeSpan.FromMilliseconds(options.DelayMilliseconds));
    await demo.RunAsync();
}

async Task RunFlickeringDemoAsync(FlickeringDemoOptions options)
{
    if (options.Flicker)
    {
        var flickeringDemo = new FlickeringDemo();
        await flickeringDemo.RunAsync();
        return;
    }

    var oNonFlickeringDemo = new NonFlickeringDemo();
    await oNonFlickeringDemo.RunAsync();
}