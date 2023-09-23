namespace DC.CodeAnalysis.Enums;

public enum SyntaxKind
{
    // Tokens
    NumberToken,
    MinusToken,
    PlusToken,
    StarToken,
    SlashToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    WhitespaceToken,
    BadToken,
    EndOfFileToken,

    // Expressions
    LiteralExpression,
    BinaryExpression,
    ParenthesizedExpression
}