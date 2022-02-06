namespace ConsolePaint.Controls.Input;

public interface IInputHandler<in TInput>
{
    public Task HandleInputAsync(TInput input);
}