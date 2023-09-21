using DC.Enums;

namespace DC;

public sealed class SyntaxToken
{
    public SyntaxKind Kind { get; }
    public int Position { get; }
    public string Text { get; }
    public object? Value { get; }

    public SyntaxToken(SyntaxKind kind, int position, string text, object? value = null)
    {
        Kind = kind;
        Position = position;
        Text = text;
        Value = value;
    }
}