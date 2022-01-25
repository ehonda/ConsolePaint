using ConsolePaint.Demos;

var demo = new BlinkingCursorDemo(25, 25,
    TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(1000));
    
await demo.Run();