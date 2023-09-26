using DC.CodeAnalysis.Enums;

namespace DC.CodeAnalysis.Syntax;

internal sealed class Lexer
{
    private readonly string _text;
    private int _position;
    private List<string> _diagnostics = new();

    public IEnumerable<string> Diagnostics => _diagnostics;

    public Lexer(string text)
    {
        _text = text;
    }

    private char Current => Peek(0);

    private char Lookahead => Peek(1);

    private char Peek(int offset)
    {
        var index = _position + offset;

        if (index >= _text.Length)
            return '\0';

        return _text[index];
    }

    private void Next()
    {
        _position++;
    }

    public SyntaxToken Lex()
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

            if (!int.TryParse(text, out var value))
                _diagnostics.Add($"The number {_text} isn't valid int32");

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

        if (char.IsLetter(Current))
        {
            var startPosition = _position;

            while (char.IsLetter(Current))
                Next();

            var length = _position - startPosition;
            var text = _text.Substring(startPosition, length);
            var kind = SyntaxFacts.GetKeywordKind(text);

            return new SyntaxToken(kind, startPosition, text);
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
            case '&':
                {
                    if (Lookahead == '&')
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, _position += 2, "&&");
                    break;
                }
            case '|':
                {
                    if (Lookahead == '|')
                        return new SyntaxToken(SyntaxKind.PipePipeToken, _position += 2, "||");
                    break;
                }
            case '=':
                {
                    if (Lookahead == '=')
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, _position += 2, "==");
                    break;
                }
            case '!':
                {
                    if (Lookahead == '=')
                        return new SyntaxToken(SyntaxKind.BangEqualsToken, _position += 2, "!=");

                    return new SyntaxToken(SyntaxKind.BangToken, _position++, "!");
                }
            default:
                break;
        }

        _diagnostics.Add($"ERROR: Bad character input: '{Current}'");

        return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1));
    }
}