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
    IdentifierToken,

    // Expressions
    LiteralExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,

    // Keywords
    TrueKeyword,
    FalseKeyword,
}