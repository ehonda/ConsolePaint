using CommandLine;
using ConsolePaint.Demos;
using ConsolePaint.IO;
using Spectre.Console;

// var demo = new BlinkingCursorDemo(25, 25,
//     TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(1000));
//     
// await demo.Run();

// var demo = new OscillatingPixelsDemo();
//
// await demo.Run();

// if (args.Length > 0 && int.TryParse(args.First(), out var ms) && ms >= 0)
// {
//     var demo = new LgbtFlagDemo(TimeSpan.FromMilliseconds(ms));
//     await demo.Run();
// }
// else
// {
//     var demo = new LgbtFlagDemo();
//     await demo.Run();
// }

// var demo = new BlinkingCursorDemoWithColorScreen(20, 20,
//     TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(250));
//     
// await demo.RunAsync();

// var demo = new BlinkingCursorDemoWithTextScreen(20, 20,
//     TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(250));
//     
// await demo.RunAsync();

// var demo = new BlinkingCursorDemoWithStyledAsciiScreen(40, 20,
//     TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(1000));
//     
// await demo.RunAsync();

// var flickering = new FlickeringDemo();
// await flickering.RunAsync();
//
// var nonFlickering = new NonFlickeringDemo();
// await nonFlickering.RunAsync();

await Parser.Default
    .ParseArguments<BlinkingCursorDemoOptions, object>(args)
    .WithParsedAsync<BlinkingCursorDemoOptions>(async options =>
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
    });