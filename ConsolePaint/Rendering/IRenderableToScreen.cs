namespace ConsolePaint.Rendering;

public interface IRenderableToScreen<out TScreenInput>
{
    public void Render(IScreen<TScreenInput> screen, TimeSpan elapsedTimeSinceLastFrame);

    public void RenderWithCurrentTime(IScreen<TScreenInput> screen, TimeSpan currentTime);
}