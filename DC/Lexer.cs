using DC.Enums;

namespace DC;

public sealed class Lexer
{
    private readonly string _text;
    private int _position;

    public Lexer(string text)
    {
        _text = text;
    }

    private char Current
    {
        get
        {
            if (_position >= _text.Length)
                return '\0';

            return _text[_position];
        }
    }

    private void Next()
    {
        _position++;
    }

    public SyntaxToken NextToken()
    {
        if (_position >= _text.Length)
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0");

        if (char.IsDigit(Current))
        {
            var startPosition = _position;

            while (char.IsDigit(Current))
                Next();

            var length = _position - startPosition;
            var text = _text.Substring(startPosition, length);
            _ = int.TryParse(text, out var value);

            return new SyntaxToken(SyntaxKind.NumberToken, startPosition, text, value);
        }

        if (char.IsWhiteSpace(Current))
        {
            var startPosition = _position;

            while (char.IsWhiteSpace(Current))
                Next();

            var length = _position - startPosition;
            var text = _text.Substring(startPosition, length);

            return new SyntaxToken(SyntaxKind.WhitespaceToken, startPosition, text);
        }

        switch (Current)
        {
            case '+':
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+");
            case '-':
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-");
            case '*':
                return new SyntaxToken(SyntaxKind.StarToken, _position++, "*");
            case '/':
                return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/");
            case '(':
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(");
            case ')':
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")");
            default:
                break;
        }

        return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1));
    }
}