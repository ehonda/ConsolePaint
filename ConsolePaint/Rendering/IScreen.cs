namespace ConsolePaint.Rendering;

// TODO: Switch Render / Draw semantics? What's the convention (if there is one)?
public interface IScreen<in TInput>
{
    public void DrawNewFrame();
    
    public void Draw(int x, int y, TInput input);

    public void Render();
}