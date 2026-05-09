namespace Lab9.Green;

public abstract class Green
{
    private string _input;

    public string Input => _input;

    protected Green(string text)
    {
        _input = text ?? string.Empty;
    }

    public abstract void Review();

    public virtual void ChangeText(string text)
    {
        _input = text ?? string.Empty;
        Review();
    }
}
