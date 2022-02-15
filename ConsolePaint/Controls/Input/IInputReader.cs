namespace ConsolePaint.Controls.Input;

public interface IInputReader<in TConsole, TInput>
{
    // TODO: Use Task<Maybe<TInput>> using a suitable functional extensions nuget
    public Task<(bool Success, TInput Input)> TryReadInputAsync(TConsole console);
}