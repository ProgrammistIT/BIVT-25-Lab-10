namespace Lab9.Green;

public abstract class Green
{
    public string Input { get; private set; }
    protected Green(string input)
    {
        Input = input ?? string.Empty;
    }

    public abstract void Review();

    public virtual void ChangeText(string text)
    {
        Input = text ?? string.Empty;
        Review();
    }
}