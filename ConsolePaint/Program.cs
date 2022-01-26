using ConsolePaint.Demos;

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

var demo = new BlinkingCursorDemoWithColorScreen(20, 20,
    TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(250));
    
await demo.RunAsync();