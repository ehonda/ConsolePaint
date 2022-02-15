namespace ConsolePaint.Controls.Input;

public class TransformInputHandler<TOriginalInput, TTransformedInput>
    : IInputHandler<TOriginalInput>
{
    private readonly Func<TOriginalInput, TTransformedInput> _transformInput;
    private readonly IInputHandler<TTransformedInput> _transformedInputHandler;

    public TransformInputHandler(
        Func<TOriginalInput, TTransformedInput> transformInput,
        IInputHandler<TTransformedInput> transformedInputHandler)
    {
        _transformInput = transformInput;
        _transformedInputHandler = transformedInputHandler;
    }

    public Task HandleInputAsync(TOriginalInput input)
        => _transformedInputHandler.HandleInputAsync(_transformInput(input));
}