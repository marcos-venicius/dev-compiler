﻿using DC.Enums;

namespace DC;

public sealed class Parser
{
    private readonly SyntaxToken[] _tokens;
    private int _position;
    private List<string> _diagnostics = new();

    public IEnumerable<string> Diagnostics => _diagnostics;

    public Parser(string text)
    {
        var tokens = new List<SyntaxToken>();

        var lexer = new Lexer(text);

        SyntaxToken token;

        do
        {
            token = lexer.NextToken();

            if (token.Kind != SyntaxKind.WhitespaceToken &&
                token.Kind != SyntaxKind.BadToken)
            {
                tokens.Add(token);
            }
        }
        while (token.Kind != SyntaxKind.EndOfFileToken);

        _tokens = tokens.ToArray();

        _diagnostics.AddRange(lexer.Diagnostics);
    }

    private SyntaxToken Peek(int offset)
    {
        var index = _position + offset;

        if (index >= _tokens.Length)
            return _tokens[^1];

        return _tokens[index];
    }

    private SyntaxToken Current => Peek(0);

    private SyntaxToken NextToken()
    {
        var current = Current;

        _position++;

        return current;
    }

    private SyntaxToken Match(SyntaxKind kind)
    {
        if (Current.Kind == kind)
            return NextToken();

        _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");

        return new SyntaxToken(kind, Current.Position, null, null);
    }

    private ExpressionSyntax ParsePrimaryExpression()
    {
        var numberToken = Match(SyntaxKind.NumberToken);

        return new NumberExpressionSyntax(numberToken);
    }

    public SyntaxTree Parse()
    {
        var expression = ParseTerm();

        var endOfFileToken = Match(SyntaxKind.EndOfFileToken);

        return new SyntaxTree(_diagnostics, expression, endOfFileToken);
    }

    private ExpressionSyntax ParseTerm()
    {
        var left = ParseFactor();

        while (Current.Kind == SyntaxKind.PlusToken ||
                Current.Kind == SyntaxKind.MinusToken)
        {
            var operatorToken = NextToken();
            var right = ParseFactor();

            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }

    private ExpressionSyntax ParseFactor()
    {
        var left = ParsePrimaryExpression();

        while (Current.Kind == SyntaxKind.StarToken ||
                Current.Kind == SyntaxKind.SlashToken)
        {
            var operatorToken = NextToken();
            var right = ParsePrimaryExpression();

            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }
}